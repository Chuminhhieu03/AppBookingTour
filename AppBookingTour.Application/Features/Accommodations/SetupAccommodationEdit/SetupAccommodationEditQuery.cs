using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.SetupAccommodationEdit
{
    public record class SetupAccommodationEditQuery(int id) : IRequest<SetupAccommodationEditDTO>;

}
