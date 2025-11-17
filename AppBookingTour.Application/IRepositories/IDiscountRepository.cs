using AppBookingTour.Application.Features.Discounts.GetDiscountsByEntityType;
using AppBookingTour.Application.Features.Discounts.SearchDiscounts;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.IRepositories
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        Task<(List<Discount> Items, int TotalCount)> SearchDiscount(SearchDiscountFilter discountFilter, int pageIndex, int pageSize);
        Task<(List<Discount> Items, int TotalCount)> GetDiscountsByEntityType(GetDiscountsByEntityTypeFilter filter, int pageIndex, int pageSize);
    }
}
