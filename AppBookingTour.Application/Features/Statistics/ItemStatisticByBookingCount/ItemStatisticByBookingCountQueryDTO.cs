
namespace AppBookingTour.Application.Features.Statistics.ItemStatisticByBookingCount;

public class ItemStatisticByBookingCountResponse
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public List<ItemStatisticByBookingCountDTO> Items { get; set; } = new();
}

public class ItemStatisticByBookingCountDTO
{
    public int ItemId { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public int totalCompletedBookings { get; set; }
    public int totalCanceledBookings { get; set; }
    public decimal cancellationRate { get; set; }
    public int ItemScheduleCount { get; set; }
    public decimal rating { get; set; }
}