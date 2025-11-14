using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.RoomTypes.GetRoomTypeById
{
    public class GetRoomTypeByIdHandler : IRequestHandler<GetRoomTypeByIdQuery, GetRoomTypeByIdDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRoomTypeByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetRoomTypeByIdDTO> Handle(GetRoomTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var roomType = await _unitOfWork.RoomTypes.GetById(request.id);
            if (roomType == null)
            {
                throw new Exception(Message.NotFound);
            }
            roomType.AmenityName = GetListAmenityName(roomType);
            var listInfoImg = await _unitOfWork.Images.GetListImageByEntityIdAndEntityType(request.id, Domain.Enums.EntityType.RoomType);
            roomType.ListInfoImage = listInfoImg;
            if (roomType.Status.HasValue)
            {
                roomType.StatusName = Constants.ActiveStatus.dctName[Convert.ToInt32(roomType.Status.Value)];
            }
            var listRoomInventory = roomType.ListRoomInventory?.OrderBy(x => -x.Id).ToList();
            roomType.ListRoomInventory = listRoomInventory;
            return new GetRoomTypeByIdDTO
            {
                RoomType = roomType,
                Success = true
            };
        }

        private string GetListAmenityName(RoomType roomType)
        {
            var listAmenityIDStr = roomType.Amenities?.Split(", ").ToList() ?? new List<string>();

            var listAmenityID = listAmenityIDStr
                .Where(x => !string.IsNullOrEmpty(x) && int.TryParse(x, out _))
                .Select(x => int.Parse(x))
                .ToList();

            if (listAmenityID.Count == 0)
                return string.Empty;

            var listAmenity = _unitOfWork.SystemParameters
                .GetListSystemParameterByListId(listAmenityID)
                .Result;

            var listAmenityName = string.Join(", ", listAmenity.Select(x => x.Name));

            return listAmenityName;
        }
    }
}

