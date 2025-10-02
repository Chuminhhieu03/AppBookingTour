using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AppBookingTour.Infrastructure.Data.Repositories;

public class TourRepository : Repository<Tour>, ITourRepository
{
    public TourRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Tour>> GetActiveTours(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => t.IsActive)
            .Include(t => t.DepartureCity)
            .Include(t => t.Type)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Tour>> SearchToursAsync(string searchTerm, int? cityId = null, decimal? maxPrice = null)
    {
        var query = _dbSet.Where(t => t.IsActive);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(t => t.Name.Contains(searchTerm) || 
                                   t.Description != null && t.Description.Contains(searchTerm));
        }

        if (cityId.HasValue)
        {
            query = query.Where(t => t.DepartureCityId == cityId.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(t => t.BasePriceAdult <= maxPrice.Value);
        }

        return await query
            .Include(t => t.DepartureCity)
            .Include(t => t.Type)
            .OrderBy(t => t.BasePriceAdult)
            .ToListAsync();
    }

    public async Task<Tour?> GetTourWithDetailsAsync(int tourId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.DepartureCity)
            .Include(t => t.Type)
            .Include(t => t.Category)
            .Include(t => t.Itineraries)
            .Include(t => t.Departures)
            .FirstOrDefaultAsync(t => t.Id == tourId && t.IsActive, cancellationToken);
    }

    public Task<Tour?> GetTourByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Tour>> GetToursByCityAsync(int cityId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Tour>> GetToursByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Tour>> GetToursByTypeAsync(int typeId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Tour>> GetToursByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Tour>> GetToursByRatingAsync(decimal minRating, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Tour>> GetPopularToursAsync(int limit = 10, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Tour>> GetMostBookedToursAsync(int limit = 10, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Tour>> GetHighestRatedToursAsync(int limit = 10, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Tour>> GetNewestToursAsync(int limit = 10, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Tour>> GetFeaturedToursAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetTotalActiveToursCountAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetAverageRatingAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetAveragePriceAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<int, int>> GetToursByMonthAsync(int year, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsTourAvailableAsync(int tourId, DateTime departureDate, int participants, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Tour>> GetAvailableToursAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
