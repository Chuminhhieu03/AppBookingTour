using AppBookingTour.Application.Features.Discounts.SearchDiscounts;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.IRepositories
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        Task<List<Discount>> SearchDiscount(SearchDiscountFilter discountFilter, int pageIndex, int pageSize);
    }
}
