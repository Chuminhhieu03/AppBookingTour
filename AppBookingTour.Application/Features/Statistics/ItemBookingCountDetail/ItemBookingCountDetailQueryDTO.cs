namespace AppBookingTour.Application.Features.Statistics.ItemBookingCountDetail;

public class ItemBookingCountDetailResponse
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public List<ItemBookingCountDetailDTO> ItemDetails { get; set; } = new();
}

public class ItemBookingCountDetailDTO
{
    public string ItemDetailName { get; set; } = string.Empty;
    public int totalCompletedBookings { get; set; }
    public int totalCanceledBookings { get; set; }
    public decimal cancellationRate { get; set; }
}