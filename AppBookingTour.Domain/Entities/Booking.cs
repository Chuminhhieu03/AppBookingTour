using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Domain.Entities;

public class Booking : BaseEntity
{
    public string BookingCode { get; set; } = null!;
    public int UserId { get; set; }
    public BookingType BookingType { get; set; }
    public int ItemId { get; set; } // FK to Tour, Hotel, or Combo
    public DateTime BookingDate { get; set; }
    public DateTime TravelDate { get; set; }
    public int NumAdults { get; set; }
    public int NumChildren { get; set; }
    public int NumInfants { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public PaymentType PaymentType { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public string? SpecialRequests { get; set; }
    public string ContactName { get; set; } = null!;
    public string ContactPhone { get; set; } = null!;
    public string ContactEmail { get; set; } = null!;

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<BookingParticipant> Participants { get; set; } = [];
    public virtual ICollection<Payment> Payments { get; set; } = [];
    public virtual ICollection<Review> Reviews { get; set; } = [];
    public virtual ICollection<PromotionUsage> PromotionUsages { get; set; } = [];
}