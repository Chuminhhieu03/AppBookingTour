using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Domain.Entities;

public class DiscountUsage : BaseEntity
{
    public int DiscountId { get; set; }
    public int UserId { get; set; }
    public int BookingId { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime UsedAt { get; set; }

    // Navigation properties
    public virtual Discount Discount { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual Booking Booking { get; set; } = null!;
}