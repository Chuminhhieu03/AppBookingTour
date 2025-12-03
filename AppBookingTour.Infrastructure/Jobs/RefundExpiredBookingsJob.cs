using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Infrastructure.Jobs;

/// <summary>
/// Background job ?? t? ??ng hoàn l?i slots/rooms cho các booking quá h?n thanh toán
/// Ch?y ??nh k? m?i 5 phút
/// </summary>
public class RefundExpiredBookingsJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RefundExpiredBookingsJob> _logger;
    private const int PAYMENT_DEADLINE_MINUTES = 15;

    public RefundExpiredBookingsJob(
        IUnitOfWork unitOfWork,
        ILogger<RefundExpiredBookingsJob> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        _logger.LogInformation("=== Starting RefundExpiredBookingsJob ===");

        try
        {
            var deadlineThreshold = DateTime.UtcNow.AddMinutes(-PAYMENT_DEADLINE_MINUTES);

            // Tìm t?t c? booking Pending quá h?n 15 phút
            var expiredBookings = await _unitOfWork.Repository<Booking>()
                .FindAsync(b =>
                    b.Status == BookingStatus.Pending
                    && b.CreatedAt <= deadlineThreshold);

            var bookingList = expiredBookings.ToList();

            if (!bookingList.Any())
            {
                _logger.LogInformation("No expired bookings found");
                return;
            }

            _logger.LogInformation("Found {Count} expired bookings to refund", bookingList.Count);

            await _unitOfWork.BeginTransactionAsync();

            int successCount = 0;
            int failCount = 0;

            foreach (var booking in bookingList)
            {
                try
                {
                    await RefundBookingAsync(booking);
                    successCount++;
                }
                catch (Exception ex)
                {
                    failCount++;
                    _logger.LogError(ex,
                        "Failed to refund booking {BookingCode}",
                        booking.BookingCode);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation(
                "=== RefundExpiredBookingsJob completed: Success={Success}, Failed={Failed} ===",
                successCount, failCount);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Critical error in RefundExpiredBookingsJob");
            throw;
        }
    }

    private async Task RefundBookingAsync(Booking booking)
    {
        _logger.LogInformation(
            "Refunding booking {BookingCode}, Type: {BookingType}",
            booking.BookingCode,
            booking.BookingType);

        switch (booking.BookingType)
        {
            case BookingType.Tour:
                await RefundTourBookingAsync(booking);
                break;
            case BookingType.Combo:
                await RefundComboBookingAsync(booking);
                break;
            case BookingType.Accommodation:
                await RefundAccommodationBookingAsync(booking);
                break;
        }

        // Update booking status
        booking.Status = BookingStatus.Cancelled;
        booking.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Repository<Booking>().Update(booking);
    }

    private async Task RefundTourBookingAsync(Booking booking)
    {
        // Tìm TourDeparture theo ItemId và TravelDate
        var departures = await _unitOfWork.Repository<TourDeparture>()
            .FindAsync(td =>
                td.TourId == booking.ItemId
                && td.DepartureDate.Date == booking.TravelDate.Date);

        var departure = departures.FirstOrDefault();
        if (departure == null)
        {
            _logger.LogWarning(
                "TourDeparture not found for booking {BookingCode}",
                booking.BookingCode);
            return;
        }

        // Hoàn l?i slots
        var totalPeople = booking.NumAdults + booking.NumChildren;
        departure.BookedSlots -= totalPeople;
        departure.AvailableSlots += totalPeople;

        // C?p nh?t status n?u tr??c ?ó là Full
        if (departure.Status == DepartureStatus.Full && departure.AvailableSlots > 0)
        {
            departure.Status = DepartureStatus.Available;
        }

        _unitOfWork.Repository<TourDeparture>().Update(departure);

        _logger.LogInformation(
            "Refunded {People} slots to TourDeparture {DepartureId}",
            totalPeople,
            departure.Id);
    }

    private async Task RefundComboBookingAsync(Booking booking)
    {
        // Tìm ComboSchedule theo ItemId và TravelDate
        var schedules = await _unitOfWork.Repository<ComboSchedule>()
            .FindAsync(cs =>
                cs.ComboId == booking.ItemId
                && cs.DepartureDate.Date == booking.TravelDate.Date);

        var schedule = schedules.FirstOrDefault();
        if (schedule == null)
        {
            _logger.LogWarning(
                "ComboSchedule not found for booking {BookingCode}",
                booking.BookingCode);
            return;
        }

        // Hoàn l?i slots
        var totalPeople = booking.NumAdults + booking.NumChildren;
        schedule.BookedSlots -= totalPeople;
        schedule.AvailableSlots += totalPeople;

        // C?p nh?t status n?u tr??c ?ó là Full
        if (schedule.Status == ComboStatus.Full && schedule.AvailableSlots > 0)
        {
            schedule.Status = ComboStatus.Available;
        }

        _unitOfWork.Repository<ComboSchedule>().Update(schedule);

        _logger.LogInformation(
            "Refunded {People} slots to ComboSchedule {ScheduleId}",
            totalPeople,
            schedule.Id);
    }

    private async Task RefundAccommodationBookingAsync(Booking booking)
    {
        // Parse RoomInventoryIds t? string "100,101,102"
        if (string.IsNullOrEmpty(booking.RoomInventoryIds))
        {
            _logger.LogWarning(
                "No RoomInventoryIds for booking {BookingCode}",
                booking.BookingCode);
            return;
        }

        var inventoryIds = booking.RoomInventoryIds
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(id => int.Parse(id.Trim()))
            .ToList();

        // Lấy tất cả RoomInventories
        var inventories = await _unitOfWork.Repository<RoomInventory>()
            .FindAsync(ri => inventoryIds.Contains(ri.Id));

        var inventoryList = inventories.ToList();

        // Hoàn lại 1 phòng cho mọi inventory
        foreach (var inventory in inventoryList)
        {
            inventory.BookedRooms += 1; // Tăng lại số phòng như cũ 
            
            _unitOfWork.Repository<RoomInventory>().Update(inventory);
        }

        _logger.LogInformation(
            "Refunded 1 room to {Count} inventories for booking {BookingCode}",
            inventoryList.Count,
            booking.BookingCode);
    }
}
