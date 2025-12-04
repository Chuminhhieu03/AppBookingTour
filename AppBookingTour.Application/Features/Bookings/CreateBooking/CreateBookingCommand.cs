using AppBookingTour.Application.Features.Bookings.Common;
using MediatR;

namespace AppBookingTour.Application.Features.Bookings.CreateBooking;

public record CreateBookingCommand(CreateBookingRequestDTO Request) : IRequest<BookingDetailsDTO>;
