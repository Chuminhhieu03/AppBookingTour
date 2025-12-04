using AppBookingTour.Application.Features.Bookings.GetUserBookings;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
using AppBookingTour.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AppBookingTour.Infrastructure.Data.Repositories;

public class BookingRepository : Repository<Booking>, IBookingRepository
{
    private readonly ApplicationDbContext _context;
    public BookingRepository(ApplicationDbContext context) : base(context) {
        _context = context; 
    }

    public async Task<Booking?> GetBookingWithDetailsAsync(int bookingId, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Include(b => b.Participants)
            .Include(b => b.Payments)
                .ThenInclude(p => p.PaymentMethod)
            .Include(b => b.DiscountUsages)
                .ThenInclude(du => du.Discount)
            .FirstOrDefaultAsync(b => b.Id == bookingId, cancellationToken);
    }

    public async Task<Booking?> GetByBookingCodeAsync(string bookingCode, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Include(b => b.Participants)
            .Include(b => b.Payments)
            .FirstOrDefaultAsync(b => b.BookingCode == bookingCode, cancellationToken);
    }

    public async Task<bool> IsBookingCodeExistsAsync(string bookingCode, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .AnyAsync(b => b.BookingCode == bookingCode, cancellationToken);
    }

    public async Task<List<Booking>> GetBookingsByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Booking>> GetExpiredPendingBookingsAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _context.Bookings
            .Where(b => b.Status == BookingStatus.Pending && b.CreatedAt.AddMinutes(15) < now)
            .ToListAsync(cancellationToken);
    }

    public async Task<(List<Booking> Bookings, int TotalCount)> GetUserBookingsWithFilterAsync(
        int userId, 
        GetUserBookingsFilter filter, 
        int pageIndex, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        IQueryable<Booking> query = _context.Bookings
            .Where(b => b.UserId == userId);

        // Apply filters
        if (filter.Status.HasValue)
        {
            query = query.Where(b => b.Status == filter.Status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var bookings = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (bookings, totalCount);
    }
}
