using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Infrastructure.Data.Repositories;
using AppBookingTour.Infrastructure.Database;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Infrastructure.Data;

/// <summary>
/// Unit of Work implementation for managing transactions and repositories
/// Implements IUnitOfWork interface from Application layer
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UnitOfWork> _logger;
    private IDbContextTransaction? _transaction;
    private bool _disposed = false;

    // Repository instances
    //private IUserRepository? _userRepository;
    private ITourRepository? _tourRepository;

    // Generic repositories cache
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(ApplicationDbContext context, ILogger<UnitOfWork> logger)
    {
        _context = context;
        _logger = logger;
    }

    #region Repository Properties

    //public IUserRepository Users =>
    //    _userRepository ??= new UserRepository(_context);

    public ITourRepository Tours =>
        _tourRepository ??= new TourRepository(_context);

    #endregion

    #region Generic Repository Access

    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        var type = typeof(T);
        
        if (_repositories.ContainsKey(type))
        {
            return (IRepository<T>)_repositories[type];
        }

        var repository = new Repository<T>(_context);
        _repositories.Add(type, repository);
        
        return repository;
    }

    #endregion

    #region Transaction Management

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Dispatch domain events before saving
            await DispatchDomainEventsAsync(cancellationToken);
            
            var result = await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Successfully saved {EntityCount} entities to database", result);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while saving changes to database");
            throw;
        }
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress");
        }

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        _logger.LogInformation("Database transaction started");
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction in progress");
        }

        try
        {
            await SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
            _logger.LogInformation("Database transaction committed successfully");
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction in progress");
        }

        try
        {
            await _transaction.RollbackAsync(cancellationToken);
            _logger.LogWarning("Database transaction rolled back");
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    #endregion

    #region Domain Events

    public async Task DispatchDomainEventsAsync(CancellationToken cancellationToken = default)
    {
        var entities = _context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        // Clear domain events from entities
        entities.ForEach(e => e.ClearDomainEvents());

        // Dispatch domain events
        foreach (var domainEvent in domainEvents)
        {
            _logger.LogInformation("Dispatching domain event: {EventType}", domainEvent.GetType().Name);
            
            // Here you would typically use a domain event dispatcher/mediator
            // For now, we'll just log the events
            // await _mediator.Publish(domainEvent, cancellationToken);
        }

        _logger.LogInformation("Dispatched {EventCount} domain events", domainEvents.Count);
    }

    #endregion

    #region Dispose Pattern

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _transaction?.Dispose();
            _context.Dispose();
            _disposed = true;
        }
    }

    #endregion
}