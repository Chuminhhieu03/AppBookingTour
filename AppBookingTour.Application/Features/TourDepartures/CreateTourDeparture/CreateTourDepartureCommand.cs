using MediatR;

namespace AppBookingTour.Application.Features.TourDepartures.CreateTourDeparture;

public record CreateTourDepartureCommand(TourDepartureRequestDTO TourDepartureRequest) : IRequest<CreateTourDepartureResponse>;