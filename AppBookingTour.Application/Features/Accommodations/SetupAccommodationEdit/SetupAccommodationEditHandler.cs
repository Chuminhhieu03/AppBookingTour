using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.SetupAccommodationEdit
{
    public class SetupAccommodationEditHandler : IRequestHandler<SetupAccommodationEditQuery, SetupAccommodationEditDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SetupAccommodationEditHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SetupAccommodationEditDTO> Handle(SetupAccommodationEditQuery request, CancellationToken cancellationToken)
        {
            var accommodation = await _unitOfWork.Accommodations.GetById(request.id);
            if (accommodation == null)
            {
                throw new Exception(Message.NotFound);
            }
            var listInfoImg = await _unitOfWork.Images.GetListImageByEntityIdAndEntityType(request.id, Domain.Enums.EntityType.Accommodation);
            accommodation.ListInfoImage = listInfoImg;
            accommodation.StatusName = Constants.ActiveStatus.dctName[Convert.ToInt32(accommodation.IsActive)];
            if (accommodation.Type.HasValue)
                accommodation.TypeName = Constants.AccommodationType.dctName[accommodation.Type.Value];
            var listRoomType = accommodation.ListRoomType?.OrderBy(x => -x.Id).ToList();
            if (listRoomType != null)
            {
                foreach (var item in listRoomType)
                {
                    if (item.Status.HasValue && Constants.RoomTypeStatus.dctName.ContainsKey((int)item.Status))
                        item.StatusName = Constants.RoomTypeStatus.dctName[(int)item.Status];
                    if (item.Status.HasValue && Constants.RoomTypeStatus.dctColor.ContainsKey((int)item.Status))
                        item.StatusColor = Constants.RoomTypeStatus.dctColor[(int)item.Status];
                    item.ListInfoImage = await _unitOfWork.Images.GetListImageByEntityIdAndEntityType(item.Id, Domain.Enums.EntityType.RoomType);
                }
            }
            accommodation.ListRoomType = listRoomType;
            var listCity = await _unitOfWork.Cities.GetAllAsync(cancellationToken);
            return new SetupAccommodationEditDTO
            {
                Accommodation = accommodation,
                ListStatus = Constants.ActiveStatus.dctName.ToList(),
                ListType = Constants.AccommodationType.dctName.ToList(),
                ListCity = listCity.OrderBy(x => x.Name).ToList(),
                Success = true
            };
        }
    }
}
