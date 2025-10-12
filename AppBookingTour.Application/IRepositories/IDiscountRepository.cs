using AppBookingTour.Application.Features.Discounts.SearchDiscounts;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Domain.IRepositories
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        Task<List<Discount>> SearchDiscount(SearchDiscountFilter discountFilter, int pageIndex, int pageSize);
        Task<>
    }
}
