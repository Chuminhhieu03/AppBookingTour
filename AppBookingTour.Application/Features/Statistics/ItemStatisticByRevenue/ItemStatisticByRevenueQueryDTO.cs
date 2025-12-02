using AppBookingTour.Application.Features.Tours.SearchTours;

namespace AppBookingTour.Application.Features.Statistics.ItemStatisticByRevenue;

public class ItemStatisticByRevenueResponse
{
    public int ItemTypeId { get; set; }
    public string ItemTypeName { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public List<ItemStatisticDTO> Items { get; set; } = new();
    public PaginationMeta Meta { get; set; } = null!;
}

public class ItemStatisticDTO
{
    public int ItemId { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public decimal rating { get; set; }
    public int totalCompletedBookings { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal averageRevenuePerBooking { get; set; }
    public int ItemOptionCount { get; set; }
}