using MediatR;

namespace AppBookingTour.Application.Features.Tours.GetTourForBooking;

/// <summary>
/// Query to get tour with specific departure for booking
/// </summary>
/// <param name="DepartureId">The tour departure ID that user selected</param>
public record GetTourForBookingQuery(int DepartureId) : IRequest<TourForBookingDTO?>;
