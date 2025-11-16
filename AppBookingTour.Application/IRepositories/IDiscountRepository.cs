using AppBookingTour.Application.Features.Discounts.SearchDiscounts;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.IRepositories
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        Task<List<Discount>> SearchDiscount(SearchDiscountFilter discountFilter, int pageIndex, int pageSize);
        Task<List<Discount>> GetDiscountsByEntityType(int entityType, string? code, string? name, int pageIndex, int pageSize);
    }
}
