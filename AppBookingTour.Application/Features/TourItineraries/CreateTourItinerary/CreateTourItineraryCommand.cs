using MediatR;

namespace AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;

public record CreateTourItineraryCommand(TourItineraryRequestDTO TourItineraryRequest) : IRequest<CreateTourItineraryResponse>;
