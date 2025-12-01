namespace AppBookingTour.Domain.Entities;

/// <summary>
/// L?u chi ti?t giá phòng t?ng ?êm cho booking accommodation
/// ??m b?o giá không thay ??i khi admin c?p nh?t RoomInventory
/// </summary>
public class BookingRoomDetail : BaseEntity
{
    public int BookingId { get; set; }
    public int RoomInventoryId { get; set; }
    public DateTime Date { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChildren { get; set; }
    public decimal BasePrice { get; set; }
    
    // Navigation properties
    public virtual Booking Booking { get; set; } = null!;
}
