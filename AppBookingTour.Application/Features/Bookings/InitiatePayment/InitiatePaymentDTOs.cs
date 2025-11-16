namespace AppBookingTour.Application.Features.Bookings.InitiatePayment;

public class InitiatePaymentRequestDTO
{
    public int BookingId { get; set; }
    public int PaymentMethodId { get; set; }
    public string IpAddress { get; set; } = null!;
}

public class InitiatePaymentResponseDTO
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public string? PaymentUrl { get; set; }
    public string? QRCodeBase64 { get; set; }
    public string? PaymentNumber { get; set; }
    public DateTime? PaymentExpiry { get; set; }
    public decimal Amount { get; set; }
}
