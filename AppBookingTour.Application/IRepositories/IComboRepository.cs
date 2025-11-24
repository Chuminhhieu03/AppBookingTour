using AppBookingTour.Application.Features.Combos.SearchCombosForCustomer;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.IRepositories;

/// <summary>
/// Repository interface for Combo entity
/// Provides specific methods for Combo operations
/// </summary>
public interface IComboRepository : IRepository<Combo>
{
    /// <summary>
    /// Get combo with all related entities (Cities, Schedules)
    /// </summary>
    Task<Combo?> GetComboWithDetailsAsync(int comboId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Check if combo has any active bookings
    /// </summary>
    Task<bool> HasActiveBookingsAsync(int comboId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get combo with schedules only
    /// </summary>
    Task<Combo?> GetComboWithSchedulesAsync(int comboId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Check if combo code exists (for validation)
    /// </summary>
    Task<bool> IsCodeExistsAsync(string code, int? excludeComboId = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Update combo cover image URL
    /// </summary>
    Task UpdateCoverImageAsync(int comboId, string? imageUrl, CancellationToken cancellationToken = default);

    Task<(List<Combo> Combos, int TotalCount)> SearchCombosForCustomerAsync(SearchCombosForCustomerFilter filter, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get featured combos (random selection with available schedules)
    /// </summary>
    Task<List<Combo>> GetFeaturedCombosAsync(int count, CancellationToken cancellationToken = default);
}
