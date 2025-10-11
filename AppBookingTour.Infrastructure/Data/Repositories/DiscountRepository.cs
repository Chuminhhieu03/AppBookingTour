using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.IRepositories;
using AppBookingTour.Infrastructure.Database;

namespace AppBookingTour.Infrastructure.Data.Repositories
{
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        public DiscountRepository(ApplicationDbContext context) : base(context) { }
    }
}
