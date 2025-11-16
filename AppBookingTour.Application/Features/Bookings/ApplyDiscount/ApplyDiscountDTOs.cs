namespace AppBookingTour.Application.Features.Bookings.ApplyDiscount;

public class ApplyDiscountRequestDTO
{
    public string DiscountCode { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public int? UserId { get; set; }
}

public class ApplyDiscountResponseDTO
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = null!;
    public string? DiscountCode { get; set; }
    public string? DiscountName { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }
}
