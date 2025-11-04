
namespace AppBookingTour.Application.Features.Statistics.ItemStatisticByRevenue;


public class ItemStatisticByRevenueResponse
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public List<ItemStatisticDTO> Items { get; set; } = new();
}

public class ItemStatisticDTO
{
    public int ItemId { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public int totalCompletedBookings { get; set; }
    public int totalCanceledBookings { get; set; }
    public decimal cancellationRate { get; set; }
    public int ItemScheduleCount { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal averageRevenuePerBooking { get; set; }
    public decimal rating { get; set; }
}