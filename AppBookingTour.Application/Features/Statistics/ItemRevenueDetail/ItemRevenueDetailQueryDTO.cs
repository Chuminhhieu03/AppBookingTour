
namespace AppBookingTour.Application.Features.Statistics.ItemRevenueDetail;

public class ItemRevenueDetailResponse
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public decimal TotalRevenue { get; set; }
    public List<ItemRevenueDetailDTO> ItemDetails { get; set; } = new();
}

public class ItemRevenueDetailDTO
{
    public string ItemDetailName { get; set; } = string.Empty;
    public int totalCompletedBookings { get; set; }
    public int totalFailedBookings { get; set; }
    public decimal TotalRevenue { get; set; }
}
