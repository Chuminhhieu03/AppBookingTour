using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.GetAccommodationForCustomerById
{
    public class GetAccommodationForCustomerByIdHandler : IRequestHandler<GetAccommodationForCustomerByIdQuery, GetAccommodationForCustomerByIdDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAccommodationForCustomerByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAccommodationForCustomerByIdDTO> Handle(GetAccommodationForCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            // Lấy accommodation
            var accommodation = await _unitOfWork.Accommodations.GetById(request.id);

            if (accommodation == null)
                throw new Exception(Message.NotFound);

            // Amenity
            accommodation.AmenityName = GetListAmenityName(accommodation.Amenities);

            // Ảnh của accommodation
            accommodation.ListInfoImage = await _unitOfWork.Images
                .GetListImageByEntityIdAndEntityType(request.id, Domain.Enums.EntityType.Accommodation);

            // Status name
            accommodation.StatusName = Constants.ActiveStatus.dctName[Convert.ToInt32(accommodation.IsActive)];

            // Type name
            if (accommodation.Type.HasValue)
                accommodation.TypeName = Constants.AccommodationType.dctName[accommodation.Type.Value];

            // ROOM TYPES - Lọc theo điều kiện:
            // 1. Status = true
            // 2. Có RoomInventory với Date = ngày hiện tại và BookedRooms > 0
            var today = DateTime.Today;
            
            // Lấy tất cả RoomInventory thỏa mãn điều kiện ngày hôm nay và BookedRooms > 0
            var validRoomInventories = await _unitOfWork.RoomInventories.FindAsync(
                ri => ri.Date.Date == today && ri.BookedRooms > 0,
                cancellationToken);
            
            var validRoomTypeIds = validRoomInventories
                .Select(ri => ri.RoomTypeId)
                .Distinct()
                .ToHashSet();
            
            var listRoomType = accommodation.ListRoomType?
                .Where(rt => rt.Status == true && validRoomTypeIds.Contains(rt.Id))
                .OrderByDescending(x => x.Id)
                .ToList();

            if (listRoomType != null)
            {
                foreach (var item in listRoomType)
                {
                    // Amenity name của room type
                    item.AmenityName = GetListAmenityName(item.Amenities);

                    // Lấy ảnh room type
                    var roomTypeImages = await _unitOfWork.Images
                        .GetListImageByEntityIdAndEntityType(item.Id, Domain.Enums.EntityType.RoomType);

                    item.ListInfoImage = roomTypeImages;
                }
            }

            accommodation.ListRoomType = listRoomType;

            return new GetAccommodationForCustomerByIdDTO
            {
                Accommodation = accommodation,
                Success = true
            };
        }
        
        private string GetListAmenityName(string amenities)
        {
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

