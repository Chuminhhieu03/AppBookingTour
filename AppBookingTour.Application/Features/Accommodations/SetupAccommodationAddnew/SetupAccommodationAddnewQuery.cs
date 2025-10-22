using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.SetupAccommodationAddnew
{
    public record class SetupAccommodationAddnewQuery() : IRequest<SetupAccommodationAddnewDTO>;

}
