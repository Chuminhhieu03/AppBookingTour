using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.IRepositories;

namespace AppBookingTour.Application.IRepositories;

/// <summary>
/// Unit of Work pattern interface for managing transactions and repositories
/// Centralizes all repository access and ensures data consistency
/// </summary>
public interface IUnitOfWork : IDisposable
{
    #region Core Repositories

    //IUserRepository Users { get; }
    ITourRepository Tours { get; }
    IDiscountRepository Discounts { get; }
    IAccommodationRepository Accommodations { get; }
    IRoomInventoryRepository RoomInventories { get; }
    IRoomTypeRepository RoomTypes { get; }
    ICityRepository Cities { get; }
    IBlogPostRepository BlogPosts { get; }
    IImageRepository Images { get; }

    #endregion
    
    #region Generic Repository Access
    /// <summary>
    /// Get generic repository for any entity type
    /// </summary>
    /// <typeparam name="T">Entity type that inherits from BaseEntity</typeparam>
    /// <returns>Generic repository instance</returns>
    IRepository<T> Repository<T>() where T : BaseEntity;
    #endregion
    
    #region Transaction Management
    /// <summary>
    /// Save all changes to the database
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of affected records</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Begin a database transaction
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Commit the current transaction
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Rollback the current transaction
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    #endregion
}