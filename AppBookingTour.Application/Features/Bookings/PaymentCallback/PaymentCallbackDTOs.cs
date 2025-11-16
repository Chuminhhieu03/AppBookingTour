namespace AppBookingTour.Application.Features.Bookings.PaymentCallback;

public class PaymentCallbackRequestDTO
{
    public Dictionary<string, string> VnPayData { get; set; } = new();
}

public class PaymentCallbackResponseDTO
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public string RedirectUrl { get; set; } = null!;
    public string? BookingCode { get; set; }
    public int? BookingId { get; set; }
    public string? TransactionId { get; set; }
    public decimal? Amount { get; set; }
}
