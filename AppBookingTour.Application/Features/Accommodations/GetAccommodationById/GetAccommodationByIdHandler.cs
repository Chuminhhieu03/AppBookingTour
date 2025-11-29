using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.GetAccommodationById
{
    public class GetAccommodationByIdHandler : IRequestHandler<GetAccommodationByIdQuery, GetAccommodationByIdDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAccommodationByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAccommodationByIdDTO> Handle(GetAccommodationByIdQuery request, CancellationToken cancellationToken)
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

            // ROOM TYPES
            var listRoomType = accommodation.ListRoomType?
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

            return new GetAccommodationByIdDTO
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
