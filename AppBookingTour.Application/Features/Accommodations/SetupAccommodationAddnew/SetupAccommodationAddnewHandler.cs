using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.SetupAccommodationAddnew
{
    public class SetupAccommodationAddnewHandler : IRequestHandler<SetupAccommodationAddnewQuery, SetupAccommodationAddnewDTO>
    {
        private IUnitOfWork _unitOfWork;
        public SetupAccommodationAddnewHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SetupAccommodationAddnewDTO> Handle(SetupAccommodationAddnewQuery request, CancellationToken cancellationToken)
        {
            var listCity = await _unitOfWork.Cities.GetAllAsync(cancellationToken);
            var listAmenity = await _unitOfWork.SystemParameters.GetListSystemParameterByFeatureCode(Domain.Enums.FeatureCode.AccommodationAmenity);
            return new SetupAccommodationAddnewDTO
            {
                ListStatus = Constants.ActiveStatus.dctName.ToList(),
                ListType = Constants.AccommodationType.dctName.ToList(),
                ListCity = listCity.OrderBy(x => x.Name).ToList(),
                ListAmenity = listAmenity,
                Success = true
            };
        }
    }
}
