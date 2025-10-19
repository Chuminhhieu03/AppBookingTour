using MediatR;

using AppBookingTour.Application.Features.TourDepartures.CreateTourDeparture;

namespace AppBookingTour.Application.Features.TourDepartures.UpdateTourDeparture;

public record UpdateTourDepartureCommand(int TourDepartureId, TourDepartureRequestDTO TourDepartureRequest) : IRequest<UpdateTourDepartureResponse>;
