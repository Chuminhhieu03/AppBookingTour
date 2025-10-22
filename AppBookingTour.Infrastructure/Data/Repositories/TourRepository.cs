using AppBookingTour.Application.Features.Tours.SearchTours;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AppBookingTour.Infrastructure.Data.Repositories;

public class TourRepository : Repository<Tour>, ITourRepository
{
    public TourRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Tour?> GetTourWithDetailsAsync(int tourId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.DepartureCity)
            .Include(t => t.Type)
            .Include(t => t.Itineraries)
            .Include(t => t.Departures)
            .FirstOrDefaultAsync(t => t.Id == tourId && t.IsActive, cancellationToken);
    }

    public async Task<(List<Tour> Tours, int TotalCount)> SearchToursAsync(SearchTourFilter filter, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        IQueryable<Tour> query = _dbSet.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Code))
        {
            query = query.Where(t => t.Code.Contains(filter.Code));
        }

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(t => t.Name.Contains(filter.Name));
        }

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(t => t.CategoryId == filter.CategoryId.Value);
        }

        if (filter.TypeId.HasValue)
        {
            query = query.Where(t => t.TypeId == filter.TypeId.Value);
        }

        if (filter.CityId.HasValue)
        {
            query = query.Where(t => t.DepartureCityId == filter.CityId.Value);
        }

        if (filter.PriceFrom.HasValue)
        {
            query = query.Where(t => t.BasePriceAdult >= filter.PriceFrom.Value);
        }

        if (filter.PriceTo.HasValue)
        {
            query = query.Where(t => t.BasePriceAdult <= filter.PriceTo.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Include(t => t.DepartureCity)
            .Include(t => t.Category)
            .Include(t => t.Type)
            .OrderBy(t => t.Id)
            .Skip((pageIndex-1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
