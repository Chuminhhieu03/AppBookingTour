using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.SetupAccommodationDefault
{
    public record class SetupAccommodationDefaultQuery() : IRequest<SetupAccommodationDefaultDTO>;

}
