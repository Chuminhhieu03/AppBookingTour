using AppBookingTour.Application.Features.TourDepartures.GetTourDepartureById;
using MediatR;

namespace AppBookingTour.Application.Features.TourDepartures.CreateTourDeparture;

public record CreateTourDepartureCommand(int TourId, TourDepartureRequestDTO TourDepartureRequest) : IRequest<TourDepartureDTO>;