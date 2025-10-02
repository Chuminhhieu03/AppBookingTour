using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Domain.Entities;

public class Payment : BaseEntity
{
    public int BookingId { get; set; }
    public int PaymentMethodId { get; set; }
    public string PaymentNumber { get; set; } = null!;
    public decimal Amount { get; set; }
    public string? TransactionId { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public string? GatewayResponse { get; set; } // JSON
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Booking Booking { get; set; } = null!;
    public virtual PaymentMethod PaymentMethod { get; set; } = null!;
}