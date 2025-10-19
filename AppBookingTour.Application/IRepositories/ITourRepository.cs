using AppBookingTour.Application.Features.Tours.CreateTour;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.IRepositories;

/// <summary>
/// Tour repository interface with tour-specific business methods
/// </summary>
public interface ITourRepository : IRepository<Tour>
{
    // Basic tour operations
    Task<IEnumerable<Tour>> GetActiveTours(CancellationToken cancellationToken = default);
    Task<Tour?> GetTourWithDetailsAsync(int tourId, CancellationToken cancellationToken = default);
    Task<Tour?> GetTourByCodeAsync(string code, CancellationToken cancellationToken = default);
    
    // Search and filtering
    Task<IEnumerable<Tour>> SearchToursAsync(string searchTerm, int? cityId = null, decimal? maxPrice = null);
    Task<IEnumerable<Tour>> GetToursByCityAsync(int cityId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Tour>> GetToursByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Tour>> GetToursByTypeAsync(int typeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Tour>> GetToursByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default);
    Task<IEnumerable<Tour>> GetToursByRatingAsync(decimal minRating, CancellationToken cancellationToken = default);
    
    // Popular and recommended tours
    Task<IEnumerable<Tour>> GetPopularToursAsync(int limit = 10, CancellationToken cancellationToken = default);
    Task<IEnumerable<Tour>> GetMostBookedToursAsync(int limit = 10, CancellationToken cancellationToken = default);
    Task<IEnumerable<Tour>> GetHighestRatedToursAsync(int limit = 10, CancellationToken cancellationToken = default);
    Task<IEnumerable<Tour>> GetNewestToursAsync(int limit = 10, CancellationToken cancellationToken = default);
    Task<IEnumerable<Tour>> GetFeaturedToursAsync(CancellationToken cancellationToken = default);
    
    // Business analytics
    Task<int> GetTotalActiveToursCountAsync(CancellationToken cancellationToken = default);
    Task<decimal> GetAverageRatingAsync(CancellationToken cancellationToken = default);
    Task<decimal> GetAveragePriceAsync(CancellationToken cancellationToken = default);
    Task<Dictionary<int, int>> GetToursByMonthAsync(int year, CancellationToken cancellationToken = default);
    
    // Availability management
    Task<bool> IsTourAvailableAsync(int tourId, DateTime departureDate, int participants, CancellationToken cancellationToken = default);
    Task<IEnumerable<Tour>> GetAvailableToursAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default); 
}