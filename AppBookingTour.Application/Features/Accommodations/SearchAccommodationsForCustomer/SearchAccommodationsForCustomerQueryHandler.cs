using AppBookingTour.Application.IRepositories;

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using AppBookingTour.Application.Features.Tours.SearchTours;
using System.Reflection.Metadata.Ecma335;
using AppBookingTour.Domain.Entities;


namespace AppBookingTour.Application.Features.Accommodations.SearchAccommodationsForCustomer
{
    public class SearchAccommodationsForCustomerQueryHandler : IRequestHandler<SearchAccommodationsForCustomerQuery, SearchAccommodationsForCustomerResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SearchAccommodationsForCustomerQueryHandler> _logger;

        public SearchAccommodationsForCustomerQueryHandler(IUnitOfWork unitOfWork, ILogger<SearchAccommodationsForCustomerQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<SearchAccommodationsForCustomerResponse> Handle(SearchAccommodationsForCustomerQuery request, CancellationToken cancellationToken)
        {
            int pageIndex = request.PageIndex ?? 1;
            int pageSize = request.PageSize ?? 10;

            _logger.LogInformation(
                "Calling SQL Repository to search accommodations. Filter: {@Filter}, Page: {Page}, PageSize: {PageSize}",
                request.Filter, pageIndex, pageSize);

            // Lấy những accommodation có roomType và roomType có roomInventory có Date trong khoảng ngày tìm kiếm
            var (accommodations, totalCount) = await _unitOfWork.Accommodations.SearchAccommodationsForCustomerAsync(
                request.Filter,
                pageIndex,
                pageSize,
                cancellationToken);

            // Lấy ra những accommodation hợp lệ: Có roomInventory đủ trong khoảng ngày
            var validListAccommodation = new List<CustomerAccommodationListItem>();
            var now = DateTime.UtcNow;
            var fromDate = request.Filter.CheckInDate ?? now;
            var toDate = request.Filter.CheckOutDate ?? now.AddDays(1);
            if(accommodations != null)
            {
                foreach(var accommodation in accommodations)
                {
                    var listRoomType = await _unitOfWork.RoomTypes.GetByAccommodationId(accommodation.Id);
                    var validRoomType = new List<RoomType>();
                    if (listRoomType != null)
                    {
                        foreach (var roomType in listRoomType)
                        {
                            var listRoomInventory = await _unitOfWork.RoomInventories.GetByRoomTypeAndDateRange(roomType.Id, fromDate, toDate);
                            if (listRoomInventory == null || listRoomInventory.Count() < (toDate.Date - fromDate.Date).Days)
                                continue;
                            validRoomType.Add(roomType);
                        }
                    }
                    if (validRoomType.Count() > 0)
                    {
                        accommodation.TotalAvailableRooms = validRoomType.Count();
                        accommodation.AmenitiesName = GetListAmenityName(accommodation.Amenities);
                        validListAccommodation.Add(accommodation);
                    }
                }
            }
            totalCount = validListAccommodation.Count();

            var totalPages = (pageSize == 0) ? 0 : (int)Math.Ceiling((double)totalCount / pageSize);

            return new SearchAccommodationsForCustomerResponse
            {
                Accommodations = validListAccommodation,
                Meta = new PaginationMeta
                {
                    TotalCount = totalCount,
                    Page = pageIndex,
                    PageSize = pageSize,
                    TotalPages = totalPages
                }
            };
        }

        private string GetListAmenityName(string amenities)
        {
            if (string.IsNullOrEmpty(amenities)) return "";
            var listAmenityIDStr = amenities?.Split(", ").ToList() ?? new List<string>();

            var listAmenityID = listAmenityIDStr
                .Select(x => int.Parse(x))
                .ToList();

            var listAmenity = _unitOfWork.SystemParameters
                .GetListSystemParameterByListId(listAmenityID)
                .Result;

            var listAmenityName = string.Join(", ", listAmenity.Select(x => x.Name));

            return listAmenityName;
        }
    }
}