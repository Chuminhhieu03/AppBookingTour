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
            var accommodation = await _unitOfWork.Accommodations.GetById(request.id);
            if (accommodation == null)
            {
                throw new Exception(Message.NotFound);
            }
            accommodation.AmenityName = GetListAmenityName(accommodation);
            var listInfoImg = await _unitOfWork.Images.GetListImageByEntityIdAndEntityType(request.id, Domain.Enums.EntityType.Accommodation);
            accommodation.ListInfoImage = listInfoImg;
            accommodation.StatusName = Constants.ActiveStatus.dctName[Convert.ToInt32(accommodation.IsActive)];
            if (accommodation.Type.HasValue)
                accommodation.TypeName = Constants.AccommodationType.dctName[accommodation.Type.Value];
            var listRoomType = accommodation.ListRoomType?.OrderBy(x => -x.Id).ToList();
            accommodation.ListRoomType = listRoomType;
            return new GetAccommodationByIdDTO
            {
                Accommodation = accommodation,
                Success = true
            };
        }

        private string GetListAmenityName(Accommodation accommodation)
        {
            var listAmenityIDStr = accommodation.Amenities?.Split(", ").ToList() ?? new List<string>();

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
