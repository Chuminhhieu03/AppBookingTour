using MediatR;

namespace AppBookingTour.Application.Features.Discounts.GetDiscountsByEntityType
{
    public record class GetDiscountsByEntityTypeQuery(int? pageIndex, int? pageSize, GetDiscountsByEntityTypeFilter? filter) : IRequest<GetDiscountsByEntityTypeResponse>;
}

