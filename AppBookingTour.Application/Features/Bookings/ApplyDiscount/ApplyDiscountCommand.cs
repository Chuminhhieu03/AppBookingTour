using MediatR;

namespace AppBookingTour.Application.Features.Bookings.ApplyDiscount;

public record ApplyDiscountCommand(ApplyDiscountRequestDTO Request) : IRequest<ApplyDiscountResponseDTO>;
