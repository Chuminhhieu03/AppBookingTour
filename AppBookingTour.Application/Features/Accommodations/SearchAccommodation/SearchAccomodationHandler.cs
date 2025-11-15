using AppBookingTour.Application.Features.Discounts.SearchDiscounts;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.SearchAccommodation
{
    internal class SearchAccomodationHandler : IRequestHandler<SearchAccomodationQuery, SearchAccommodationResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchAccomodationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SearchAccommodationResponse> Handle(SearchAccomodationQuery request, CancellationToken cancellationToken)
        {
            var filter = request.SearchAccommodationFilter ?? new SearchAccommodationFilter();
            int pageIndex = request.pageIndex ?? Constants.Pagination.PageIndex;
            int pageSize = request.pageSize ?? Constants.Pagination.PageSize;
            var result = await _unitOfWork.Accommodations.SearchAccommodation(filter, pageIndex, pageSize);
            var listAccommodation = result.ListAccommodation;
            var totalCount = result.TotalCount;
            listAccommodation?.ForEach(item =>
            {
                item.StatusName = Constants.ActiveStatus.dctName[Convert.ToInt32(item.IsActive)];
                if (item.Type.HasValue && Constants.AccommodationType.dctName.ContainsKey(item.Type.Value))
                    item.TypeName = Constants.AccommodationType.dctName[item.Type.Value];
            });
            return new SearchAccommodationResponse
            {
                Success = true,
                ListAccommodation = listAccommodation,
                TotalCount = totalCount
            };
        }
    }
}
