using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.Discounts.SetupDiscountEdit
{
    public record class SetupDiscountEditQuery(int id) : IRequest<SetupDiscountEditDTO>;

}
