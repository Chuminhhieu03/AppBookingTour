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
            var listInfoImg = await _unitOfWork.Images.GetListAccommodationImageByEntityId(request.id);
            accommodation.ListInfoImage = listInfoImg;
            accommodation.StatusName = Constants.ActiveStatus.dctName[Convert.ToInt32(accommodation.IsActive)];
            if (accommodation.Type.HasValue)
                accommodation.TypeName = Constants.AccommodationType.dctName[accommodation.Type.Value];
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
