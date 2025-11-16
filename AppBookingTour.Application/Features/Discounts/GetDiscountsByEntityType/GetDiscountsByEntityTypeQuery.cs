using MediatR;

namespace AppBookingTour.Application.Features.Discounts.GetDiscountsByEntityType
{
    public record class GetDiscountsByEntityTypeQuery(
        int EntityType,
        int? pageIndex,
        int? pageSize,
        string? Code,
        string? Name
    ) : IRequest<GetDiscountsByEntityTypeResponse>;
}

