using AppBookingTour.Application.Features.TourTypes.SearchTourType;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AppBookingTour.Infrastructure.Data.Repositories;

public class TourTypeRepository : Repository<TourType>, ITourTypeRepository
{
    public TourTypeRepository(ApplicationDbContext context) : base(context) { }

    public async Task<(List<TourType> TourTypes, int TotalCount)> SearchTourTypesAsync(
        TourTypeFilter filter,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TourType> query = _dbSet.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(tt => tt.Name.ToLower().Contains(filter.Name.ToLower()));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(tt => tt.Name)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}