using MediatR;

namespace AppBookingTour.Application.Features.Bookings.InitiatePayment;

public record InitiatePaymentCommand(InitiatePaymentRequestDTO Request) : IRequest<InitiatePaymentResponseDTO>;
