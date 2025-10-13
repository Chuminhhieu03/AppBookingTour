using MediatR;

namespace AppBookingTour.Application.Features.Discounts.UpdateDiscount
{
    public record UpdateDiscountCommand(int DiscountId, UpdateDiscountDTO Discount) : IRequest<UpdateDiscountResponse>;
}
