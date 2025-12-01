namespace AppBookingTour.Application.Features.Bookings.ApplyDiscount;

public class ApplyDiscountRequestDTO
{
    public string DiscountCode { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public string BookingType { get; set; } = null!; // "tour", "combo", "accommodation"
    public int ItemId { get; set; }
    // Removed: UserId - will be fetched from JWT in handler
}

public class ApplyDiscountResponseDTO
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = null!;
    public string? DiscountCode { get; set; }
    public string? DiscountName { get; set; }
    public string? DiscountType { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public DateTime? ValidUntil { get; set; }
    public string? ApplicableFor { get; set; }
}
