using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.UpdateAccommodation
{
    public record UpdateAccommodationCommand(int AccommodationId, UpdateAccommodationDTO Accommodation) : IRequest<UpdateAccommodationResponse>;
}
