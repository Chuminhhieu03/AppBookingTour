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
}
