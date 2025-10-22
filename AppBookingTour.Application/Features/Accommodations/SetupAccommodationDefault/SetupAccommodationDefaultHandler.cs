using AppBookingTour.Domain.Constants;
using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.SetupAccommodationDefault
{
    public class SetupAccommodationDefaultHandler : IRequestHandler<SetupAccommodationDefaultQuery, SetupAccommodationDefaultDTO>
    {
        public async Task<SetupAccommodationDefaultDTO> Handle(SetupAccommodationDefaultQuery request, CancellationToken cancellationToken)
        {
            return new SetupAccommodationDefaultDTO
            {
                ListStatus = Constants.ActiveStatus.dctName.ToList(),
                ListType = Constants.AccommodationType.dctName.ToList(),
                Success = true
            };
        }
    }
}
