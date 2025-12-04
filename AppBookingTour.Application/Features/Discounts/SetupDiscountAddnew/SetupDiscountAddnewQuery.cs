using MediatR;

namespace AppBookingTour.Application.Features.Discounts.SetupDiscountAddnew
{
    public record class SetupDiscountAddnewQuery() : IRequest<SetupDiscountAddnewDTO>;

}
