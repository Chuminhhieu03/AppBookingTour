using MediatR;

namespace AppBookingTour.Application.Features.TourDepartures.GetListTourDeparture;

public record GetTourDeparturesByTourIdQuery(int TourId) : IRequest<List<ListTourDepartureItem>>;