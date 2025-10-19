using MediatR;

using AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;

namespace AppBookingTour.Application.Features.TourItineraries.UpdateTourItinerary;

public record UpdateTourItineraryCommand(int TourItineraryId, TourItineraryRequestDTO TourItineraryRequest) : IRequest<UpdateTourItineraryResponse>;
