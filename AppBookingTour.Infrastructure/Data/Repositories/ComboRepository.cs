using AppBookingTour.Application.Features.Combos.SearchCombosForCustomer;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
using AppBookingTour.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AppBookingTour.Infrastructure.Data.Repositories;

/// <summary>
/// Repository implementation for Combo entity
/// </summary>
public class ComboRepository : Repository<Combo>, IComboRepository
{
    private readonly ApplicationDbContext _context;

    public ComboRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// Get combo with all related entities (Cities, Schedules)
    /// </summary>
    public async Task<Combo?> GetComboWithDetailsAsync(int comboId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.FromCity)
            .Include(c => c.ToCity)
            .Include(c => c.Schedules)
            .FirstOrDefaultAsync(c => c.Id == comboId, cancellationToken);
    }

    /// <summary>
    /// Check if combo has any active bookings (not cancelled)
    /// </summary>
    public async Task<bool> HasActiveBookingsAsync(int comboId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Booking>()
            .AnyAsync(b => b.BookingType == BookingType.Combo
                && b.ItemId == comboId
                && b.Status != BookingStatus.Cancelled,
                cancellationToken);
    }

    /// <summary>
    /// Get combo with schedules only (lighter query)
    /// </summary>
    public async Task<Combo?> GetComboWithSchedulesAsync(int comboId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Schedules)
            .FirstOrDefaultAsync(c => c.Id == comboId, cancellationToken);
    }

    /// <summary>
    /// Check if combo code exists (excluding a specific combo ID for updates)
    /// </summary>
    public async Task<bool> IsCodeExistsAsync(string code, int? excludeComboId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(c => c.Code == code);
        
        if (excludeComboId.HasValue)
        {
            query = query.Where(c => c.Id != excludeComboId.Value);
        }
        
        return await query.AnyAsync(cancellationToken);
    }

    /// <summary>
    /// Update combo cover image URL
    /// </summary>
    public async Task UpdateCoverImageAsync(int comboId, string? imageUrl, CancellationToken cancellationToken = default)
    {
        var combo = await _dbSet.FirstOrDefaultAsync(c => c.Id == comboId, cancellationToken);
        
        if (combo != null)
        {
            combo.ComboImageCoverUrl = imageUrl;
            combo.UpdatedAt = DateTime.UtcNow;
        }
    }

    public async Task<(List<Combo> Combos, int TotalCount)> SearchCombosForCustomerAsync(SearchCombosForCustomerFilter filter, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        IQueryable<Combo> query = _dbSet.AsQueryable();

        query = query.Where(c => c.IsActive == true);

        if (filter.PriceFrom.HasValue)
        {
            query = query.Where(c => c.BasePriceAdult >= filter.PriceFrom.Value);
        }

        if (filter.PriceTo.HasValue)
        {
            query = query.Where(c => c.BasePriceAdult <= filter.PriceTo.Value);
        }

        if (filter.DepartureCityId.HasValue)
        {
            query = query.Where(c => c.FromCityId == filter.DepartureCityId.Value);
        }

        if (filter.DestinationCityId.HasValue)
        {
            query = query.Where(c => c.ToCityId == filter.DestinationCityId.Value);
        }

        if (filter.Vehicle.HasValue)
        {
            query = query.Where(c => c.Vehicle == filter.Vehicle.Value);
        }

        if (filter.DepartureDate.HasValue)
        {
            var filterDate = filter.DepartureDate.Value;
            query = query.Where(c => c.Schedules.Any(s =>
                s.Status == ComboStatus.Available && 
                DateOnly.FromDateTime(s.DepartureDate) >= filterDate
            ));
        }
        else
        {
            query = query.Where(c => c.Schedules.Any(s => s.Status == ComboStatus.Available));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Include(c => c.FromCity)
            .Include(c => c.ToCity)
            .Include(c => c.Schedules)
            .OrderByDescending(c => c.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
