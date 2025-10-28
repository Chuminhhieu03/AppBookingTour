using MediatR;

namespace AppBookingTour.Application.Features.TourItineraries.GetListTourItinerary;

public record GetTourItinerariesByTourIdQuery(int TourId) : IRequest<List<TourItineraryListItem>>;