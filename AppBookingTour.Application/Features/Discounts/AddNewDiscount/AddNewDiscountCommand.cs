using MediatR;

namespace AppBookingTour.Application.Features.Discounts.AddNewDiscount
{
    public record AddNewDiscountCommand(AddNewDiscountDTO Discount) : IRequest<AddNewDiscountResponse>;
}
