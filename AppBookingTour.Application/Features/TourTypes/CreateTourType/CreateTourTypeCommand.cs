using MediatR;

namespace AppBookingTour.Application.Features.TourTypes.CreateTourType
{
    public record CreateTourTypeCommand(TourTypeRequestDTO RequestDto) : IRequest<CreateTourTypeResponse>;
}