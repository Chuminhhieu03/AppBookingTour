using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.AddNewAccommodation
{
    public record AddNewAccommodationCommand(AddNewAccommodationDTO Accommodation) : IRequest<AddNewAccommodationResponse>;
}
