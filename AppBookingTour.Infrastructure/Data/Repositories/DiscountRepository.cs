using AppBookingTour.Application.Features.Discounts.SearchDiscounts;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AppBookingTour.Infrastructure.Data.Repositories
{
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        public DiscountRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<Discount>> SearchDiscount(SearchDiscountFilter filter, int pageIndex, int pageSize)
        {
            IQueryable<Discount> query = _dbSet;
            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(x => x.Name.Contains(filter.Name));
            if (!string.IsNullOrEmpty(filter.Code))
                query = query.Where(x => x.Code.Contains(filter.Code));
            if (filter.Status.HasValue)
                query = query.Where(x => x.Status == filter.Status.Value);
            query = query
                .OrderBy(x => x.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);
            return await query.ToListAsync();
        }
    }
}
