using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.SetupAccommodationDisplay
{
    public record class SetupAccommodationDisplayQuery(int id) : IRequest<SetupAccommodationDisplayDTO>;

}
