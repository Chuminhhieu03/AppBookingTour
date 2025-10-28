using AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;
using MediatR;

namespace AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;

public record CreateTourItineraryCommand(TourItineraryRequestDTO TourItineraryRequest) : IRequest<TourItineraryDTO>;
