using MediatR;

namespace AppBookingTour.Application.Features.Bookings.PaymentCallback;

public record PaymentCallbackCommand(PaymentCallbackRequestDTO Request) : IRequest<PaymentCallbackResponseDTO>;
