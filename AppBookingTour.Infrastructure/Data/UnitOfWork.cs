using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.IRepositories;
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
    private ITourCategoryRepository? _tourCategoryRepository;
    private ITourTypeRepository? _tourTypeRepository;
    private IDiscountRepository _discountRepository;
    private IAccommodationRepository _accommodationRepository;
    private IRoomInventoryRepository _roomInventoryRepository;
    private IRoomTypeRepository _roomTypeRepository;
    private ICityRepository _cityRepository;
    private IBlogPostRepository? _blogPostRepository;
    private IImageRepository _imageRepository;
    private ISystemParameterRepository _systemParameterRepository;
    private IBookingRepository _bookingRepository;
    private IComboRepository? _comboRepository;
    private IRepository<Payment>? _paymentRepository;
    private IRepository<PaymentMethod>? _paymentMethodRepository;
    private IRepository<Promotion>? _promotionRepository;
    private IRepository<PromotionUsage>? _promotionUsageRepository;
    private IRepository<DiscountUsage>? _discountUsageRepository;
    private IRepository<BookingParticipant>? _bookingParticipantRepository;
    private IRepository<TourDeparture>? _tourDepartureRepository;

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

    public ITourCategoryRepository TourCategories =>
        _tourCategoryRepository ??= new TourCategoryRepository(_context);

    public ITourTypeRepository TourTypes =>
        _tourTypeRepository ??= new TourTypeRepository(_context);

    public IDiscountRepository Discounts => _discountRepository ?? new DiscountRepository(_context);
    public IAccommodationRepository Accommodations => _accommodationRepository ?? new AccomodationRepository(_context);
    public IRoomTypeRepository RoomTypes => _roomTypeRepository ?? new RoomTypeRepository(_context);
    public IRoomInventoryRepository RoomInventories => _roomInventoryRepository ?? new RoomInventoryRepository(_context);
    public ICityRepository Cities => _cityRepository ?? new CityRepository(_context);
    public IBlogPostRepository BlogPosts => _blogPostRepository ??= new BlogPostRepository(_context);
    public IImageRepository Images => _imageRepository ?? new ImageRepository(_context);
    public ISystemParameterRepository SystemParameters => _systemParameterRepository ?? new SystemParameterRepository(_context);
    public IBookingRepository Bookings => _bookingRepository ?? new BookingRepository(_context);
    public IComboRepository Combos => _comboRepository ??= new ComboRepository(_context);
    
    public IRepository<Payment> Payments => _paymentRepository ??= new Repository<Payment>(_context);
    public IRepository<PaymentMethod> PaymentMethods => _paymentMethodRepository ??= new Repository<PaymentMethod>(_context);
    public IRepository<Promotion> Promotions => _promotionRepository ??= new Repository<Promotion>(_context);
    public IRepository<PromotionUsage> PromotionUsages => _promotionUsageRepository ??= new Repository<PromotionUsage>(_context);
    public IRepository<DiscountUsage> DiscountUsages => _discountUsageRepository ??= new Repository<DiscountUsage>(_context);
    public IRepository<BookingParticipant> BookingParticipants => _bookingParticipantRepository ??= new Repository<BookingParticipant>(_context);
    public IRepository<TourDeparture> TourDepartures => _tourDepartureRepository ??= new Repository<TourDeparture>(_context);

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