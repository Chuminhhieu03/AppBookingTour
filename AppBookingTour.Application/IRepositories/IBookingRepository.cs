using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.IRepositories;

public interface IBookingRepository : IRepository<Booking>
{
    Task<Booking?> GetBookingWithDetailsAsync(int bookingId, CancellationToken cancellationToken = default);
    Task<Booking?> GetByBookingCodeAsync(string bookingCode, CancellationToken cancellationToken = default);
    Task<bool> IsBookingCodeExistsAsync(string bookingCode, CancellationToken cancellationToken = default);
    Task<List<Booking>> GetBookingsByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<List<Booking>> GetExpiredPendingBookingsAsync(CancellationToken cancellationToken = default);
}
