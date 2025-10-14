using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.Discounts.SearchDiscounts
{
    public record class SearchDiscountQuery(int? pageIndex, int? pageSize, SearchDiscountFilter? discountFilter) : IRequest<SearchDiscountResponse>;
}
