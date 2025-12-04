using AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;
using MediatR;

namespace AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;

public record CreateTourItineraryCommand(int TourId, TourItineraryRequestDTO TourItineraryRequest) : IRequest<TourItineraryDTO>;
