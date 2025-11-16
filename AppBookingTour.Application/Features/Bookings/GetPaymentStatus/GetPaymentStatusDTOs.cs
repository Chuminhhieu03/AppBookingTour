using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.Bookings.GetPaymentStatus;

public class PaymentStatusDTO
{
    public int BookingId { get; set; }
    public string BookingCode { get; set; } = null!;
    public BookingStatus BookingStatus { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public List<PaymentHistoryDTO> PaymentHistory { get; set; } = [];
    public DateTime? LastPaymentDate { get; set; }
    public DateTime? PaymentDeadline { get; set; }
    public bool IsExpired { get; set; }
}

public class PaymentHistoryDTO
{
    public int Id { get; set; }
    public string PaymentNumber { get; set; } = null!;
    public decimal Amount { get; set; }
    public string PaymentMethodName { get; set; } = null!;
    public PaymentStatus Status { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? TransactionId { get; set; }
}
