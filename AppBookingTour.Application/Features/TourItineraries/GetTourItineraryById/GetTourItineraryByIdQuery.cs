using MediatR;

namespace AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;

public record GetTourItineraryByIdQuery(int TourItineraryId) : IRequest<GetTourItineraryByIdResponse>;
