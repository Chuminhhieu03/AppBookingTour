using AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;
using AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;
using MediatR;

namespace AppBookingTour.Application.Features.TourItineraries.UpdateTourItinerary;

public record UpdateTourItineraryCommand(int TourItineraryId, TourItineraryRequestDTO TourItineraryRequest) : IRequest<TourItineraryDTO>;
