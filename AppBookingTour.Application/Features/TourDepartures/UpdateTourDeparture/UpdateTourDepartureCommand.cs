using MediatR;

using AppBookingTour.Application.Features.TourDepartures.CreateTourDeparture;
using AppBookingTour.Application.Features.TourDepartures.GetTourDepartureById;

namespace AppBookingTour.Application.Features.TourDepartures.UpdateTourDeparture;

public record UpdateTourDepartureCommand(int TourDepartureId, TourDepartureRequestDTO TourDepartureRequest) : IRequest<TourDepartureDTO>;
