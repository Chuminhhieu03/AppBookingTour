using AppBookingTour.Application.Features.Tours.SearchTours;
using AppBookingTour.Application.Features.Tours.SearchToursForCustomer;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
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
            .Include(t => t.DestinationCity)
            .Include(t => t.Type)
            .Include(t => t.Category)
            .Include(t => t.Itineraries)
            .Include(t => t.Departures)
            .FirstOrDefaultAsync(t => t.Id == tourId, cancellationToken);
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

        if (filter.Active.HasValue)
        {
            query = query.Where(t => t.IsActive == filter.Active.Value);
        }    

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Include(t => t.DepartureCity)
            .Include(t => t.Category)
            .Include(t => t.Type)
            .OrderBy(t => t.Id)
            .Skip((pageIndex-1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(List<Tour> Tours, int TotalCount)> SearchToursForCustomerAsync(SearchToursForCustomerFilter filter, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        IQueryable<Tour> query = _dbSet.AsQueryable();

        query = query.Where(t => t.IsActive == true);

        if (filter.PriceFrom.HasValue)
        {
            query = query.Where(t => t.BasePriceAdult >= filter.PriceFrom.Value);
        }

        if (filter.PriceTo.HasValue)
        {
            query = query.Where(t => t.BasePriceAdult <= filter.PriceTo.Value);
        }

        if (filter.DepartureCityId.HasValue)
        {
            query = query.Where(t => t.DepartureCityId == filter.DepartureCityId.Value);
        }

        if (filter.DestinationCityId.HasValue)
        {
            query = query.Where(t => t.DestinationCityId == filter.DestinationCityId.Value);
        }

        if (filter.TourTypeId.HasValue)
        {
            query = query.Where(t => t.TypeId == filter.TourTypeId.Value);
        }

        if (filter.TourCategoryId.HasValue)
        {
            query = query.Where(t => t.CategoryId == filter.TourCategoryId.Value);
        }

        if (filter.DepartureDate.HasValue)
        {
            var filterDate = filter.DepartureDate.Value;
            query = query.Where(t => t.Departures.Any(d =>
                d.Status == DepartureStatus.Available &&
                DateOnly.FromDateTime(d.DepartureDate) >= filterDate
            ));
        }
        else
        {
            query = query.Where(t => t.Departures.Any(d => d.Status == DepartureStatus.Available));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Include(t => t.DepartureCity)
            .Include(t => t.DestinationCity)
            .Include(t => t.Departures)
            .Include(t => t.Category)
            .Include(t => t.Type)
            .OrderByDescending(t => t.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<List<Tour>> GetFeaturedToursAsync(int count, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        // Only get active tours
        query = query.Where(t => t.IsActive == true);

        // Only get tours with available departures
        query = query.Where(t => t.Departures.Any(d => 
            d.Status == DepartureStatus.Available && 
            d.DepartureDate >= DateTime.UtcNow && 
            d.AvailableSlots > 0
        ));

        var items = await query
            .Include(t => t.DepartureCity)
            .Include(t => t.DestinationCity)
            .Include(t => t.Category)
            .Include(t => t.Type)
            .Include(t => t.Departures.Where(d => 
                d.Status == DepartureStatus.Available && 
                d.DepartureDate >= DateTime.UtcNow && 
                d.AvailableSlots > 0
            ))
            .OrderBy(t => Guid.NewGuid())
            .Take(count)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return items;
    }
}
