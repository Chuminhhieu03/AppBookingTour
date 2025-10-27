using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.RoomTypes.SetupRoomTypeDisplay
{
    public class SetupRoomTypeDisplayHandler : IRequestHandler<SetupRoomTypeDisplayQuery, SetupRoomTypeDisplayDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SetupRoomTypeDisplayHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SetupRoomTypeDisplayDTO> Handle(SetupRoomTypeDisplayQuery request, CancellationToken cancellationToken)
        {
            var roomType = await _unitOfWork.RoomTypes.GetByIdAsync(request.id);
            if (roomType == null)
            {
                throw new Exception(Message.NotFound);
            }
            roomType.AmenityName = GetListAmenityName(roomType);
            var listInfoImg = await _unitOfWork.Images.GetListImageByEntityIdAndEntityType(request.id, Domain.Enums.EntityType.RoomType);
            roomType.ListInfoImage = listInfoImg;
            if (roomType.Status.HasValue && Constants.RoomTypeStatus.dctName.ContainsKey(roomType.Status.Value))
                roomType.StatusName = Constants.RoomTypeStatus.dctName[roomType.Status.Value];
            if (roomType.Status.HasValue && Constants.RoomTypeStatus.dctColor.ContainsKey(roomType.Status.Value))
                roomType.StatusColor = Constants.RoomTypeStatus.dctColor[roomType.Status.Value];
            return new SetupRoomTypeDisplayDTO
            {
                RoomType = roomType,
                Success = true
            };
        }

        private string GetListAmenityName(RoomType roomType)
        {
            var listAmenityIDStr = roomType.Amenities?.Split(", ").ToList() ?? new List<string>();

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
