using AppBookingTour.Application.Features.Bookings.Common;
using MediatR;

namespace AppBookingTour.Application.Features.Bookings.GetBookingById;

public record GetBookingByIdQuery(int BookingId) : IRequest<BookingDetailsDTO>;
