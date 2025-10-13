using MediatR;

namespace AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;

public record CreateTourItineraryCommand(TourItineraryRequestDto TourItineraryRequest) : IRequest<CreateTourItineraryCommandResponse>;
