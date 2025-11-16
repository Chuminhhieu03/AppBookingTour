using MediatR;

namespace AppBookingTour.Application.Features.Bookings.GetPaymentStatus;

public record GetPaymentStatusQuery(int BookingId) : IRequest<PaymentStatusDTO>;
