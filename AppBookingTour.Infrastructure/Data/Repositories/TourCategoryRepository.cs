using AppBookingTour.Application.Features.TourCategories.SearchTourCategory;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AppBookingTour.Infrastructure.Data.Repositories;

public class TourCategoryRepository : Repository<TourCategory>, ITourCategoryRepository
{
    public TourCategoryRepository(ApplicationDbContext context) : base(context) { }

    public async Task<(List<TourCategory> Categories, int TotalCount)> SearchTourCategoriesAsync(
        TourCategoryFilter filter,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TourCategory> query = _dbSet.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(tc => tc.Name.ToLower().Contains(filter.Name.ToLower()));
        }

        if (filter.ParentCategoryId.HasValue)
        {
            query = query.Where(tc => tc.ParentCategoryId == filter.ParentCategoryId.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Include(tc => tc.ParentCategory)
            .OrderBy(tc => tc.Name)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}