using MediatR;

namespace AppBookingTour.Application.Features.TourDepartures.DeleteTourDeparture;

public record DeleteTourDepartureCommand(int Id) : IRequest<Unit>;
