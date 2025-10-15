using MediatR;

namespace AppBookingTour.Application.Features.TourItineraries.DeleteTourItinerary;

public record DeleteTourItineraryCommand(int TourItineraryId) : IRequest<DeleteTourItineraryResponse>;
