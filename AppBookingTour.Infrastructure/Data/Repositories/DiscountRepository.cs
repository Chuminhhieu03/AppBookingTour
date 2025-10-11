using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.IRepositories;
using AppBookingTour.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AppBookingTour.Infrastructure.Data.Repositories
{
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        public DiscountRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<Discount>> SearchDiscount(Discount filter, int pageIndex, int pageSize)
        {
            filter = filter ?? new Discount();
            IQueryable<Discount> query = _dbSet;
            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(x => x.Name.Contains(filter.Name));
            if (!filter.Status.HasValue)
                query = query.Where(x => x.Status == filter.Status.Value);
            query = query
                .OrderBy(x => x.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
            return await query.ToListAsync();
        }
    }
}
