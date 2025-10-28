using MediatR;

namespace AppBookingTour.Application.Features.TourDepartures.GetTourDepartureById;

public record GetTourDepartureByIdQuery(int TourDepartureId) : IRequest<TourDepartureDTO>;
