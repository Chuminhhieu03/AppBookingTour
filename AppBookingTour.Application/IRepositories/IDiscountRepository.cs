using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Domain.IRepositories
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        Task<List<Discount>> SearchDiscount(Discount discount, int pageIndex, int pageSize);
    }
}
