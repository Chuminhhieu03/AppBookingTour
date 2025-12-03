using MediatR;

namespace AppBookingTour.Application.Features.Bookings.GetUserBookings;

public record GetUserBookingsQuery(int UserId, int? PageIndex, int? PageSize, GetUserBookingsFilter Filter) : IRequest<GetUserBookingsResponse>;
