using AppBookingTour.Application.Features.Discounts.GetDiscountsByEntityType;
using AppBookingTour.Application.Features.Discounts.SearchDiscounts;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AppBookingTour.Infrastructure.Data.Repositories
{
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        public DiscountRepository(ApplicationDbContext context) : base(context) { }

        public async Task<(List<Discount> Items, int TotalCount)> SearchDiscount(SearchDiscountFilter filter, int pageIndex, int pageSize)
        {
            IQueryable<Discount> query = _dbSet;
            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(x => x.Name.Contains(filter.Name));
            if (!string.IsNullOrEmpty(filter.Code))
                query = query.Where(x => x.Code.Contains(filter.Code));
            if (filter.Status.HasValue)
                query = query.Where(x => x.Status == filter.Status.Value);
            
            // Get total count before pagination
            var totalCount = await query.CountAsync();
            
            var items = await query
                .OrderBy(x => -x.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            return (items, totalCount);
        }

        public async Task<(List<Discount> Items, int TotalCount)> GetDiscountsByEntityType(GetDiscountsByEntityTypeFilter filter, int pageIndex, int pageSize)
        {
            var query = _dbSet
                .Where(x => x.Status == Constants.ActiveStatus.Active);

            if (filter.EntityType.HasValue)
                query = query.Where(x => x.ServiceType == filter.EntityType.Value);

            if (!string.IsNullOrEmpty(filter.Code))
                query = query.Where(x => x.Code != null && x.Code.Contains(filter.Code));

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(x => x.Name != null && x.Name.Contains(filter.Name));

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            var result = await query
                .OrderBy(x => x.Code)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(d => new Discount
                {
                    Id = d.Id,
                    Code = d.Code,
                    Name = d.Name,
                    DiscountPercent = d.DiscountPercent,
                    Checked = d.ItemDiscounts.Any(ed =>
                        ed.ItemId == filter.EntityId &&
                        ed.ItemType == filter.EntityType)
                })
                .ToListAsync();

            return (result, totalCount);
        }
    }
}
