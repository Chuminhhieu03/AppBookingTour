using MediatR;

namespace AppBookingTour.Application.Features.ItemDiscounts.AssignDiscount;

public record AssignDiscountCommand(AssignDiscountRequestDTO Request) : IRequest<AssignDiscountResponse>;

