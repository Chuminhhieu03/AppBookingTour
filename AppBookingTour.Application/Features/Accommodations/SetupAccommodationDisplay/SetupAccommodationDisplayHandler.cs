using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.SetupAccommodationDisplay
{
    public class SetupAccommodationDisplayHandler : IRequestHandler<SetupAccommodationDisplayQuery, SetupAccommodationDisplayDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SetupAccommodationDisplayHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SetupAccommodationDisplayDTO> Handle(SetupAccommodationDisplayQuery request, CancellationToken cancellationToken)
        {
            var accommodation = await _unitOfWork.Accommodations.GetById(request.id);
            if (accommodation == null)
            {
                throw new Exception(Message.NotFound);
            }
            accommodation.StatusName = Constants.ActiveStatus.dctName[Convert.ToInt32(accommodation.IsActive)];
            if (accommodation.Type.HasValue)
                accommodation.TypeName = Constants.AccommodationType.dctName[accommodation.Type.Value];
            return new SetupAccommodationDisplayDTO
            {
                Accommodation = accommodation,
                Success = true
            };
        }
    }
}
