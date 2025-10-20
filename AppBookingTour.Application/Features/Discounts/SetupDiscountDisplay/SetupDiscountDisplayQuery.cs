using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.Discounts.SetupDiscountDisplay
{
    public record class SetupDiscountDisplayQuery(int id) : IRequest<SetupDiscountDisplayDTO>;

}
