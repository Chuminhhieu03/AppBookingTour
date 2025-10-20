using MediatR;

namespace AppBookingTour.Application.Features.Discounts.SetupDiscountDefault
{
    public record class SetupDiscountDefaultQuery() : IRequest<SetupDiscountDefaultDTO>;

}
