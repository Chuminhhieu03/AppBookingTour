using AppBookingTour.Application.Features.Tours.SearchTours;

namespace AppBookingTour.Application.Features.Statistics.ItemStatisticByBookingCount;

public class ItemStatisticByBookingCountResponse
{
    public int ItemTypeId { get; set; }
    public string ItemTypeName { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public List<ItemStatisticByBookingCountDTO> Items { get; set; } = new();
    public PaginationMeta Meta { get; set; } = null!;
}

public class ItemStatisticByBookingCountDTO
{
    public int ItemId { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public decimal rating { get; set; }
    public int totalCompletedBookings { get; set; }
    public int totalCanceledBookings { get; set; }
    public decimal cancellationRate { get; set; }
    public int ItemOptionCount { get; set; }
}