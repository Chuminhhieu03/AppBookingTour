
namespace AppBookingTour.Application.Features.Statistics.OverviewStatistic;

public class OverviewStatisticDTO
{
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal PreviousMonthRevenue { get; set; }
    public double GrowthRate { get; set; }
    public int TotalOrders { get; set; }
    public int CompletedOrders { get; set; }
    public int CanceledOrders { get; set; }
    public SummaryByTypeDTO? SummaryByType { get; set; }
    public List<MonthlyReportDTO>? MonthlyReport { get; set; }
}

public class SummaryByTypeDTO
{
    public TypeStatisticDTO? Tour { get; set; }
    public TypeStatisticDTO? Combo { get; set; }
    public TypeStatisticDTO? Accommodation { get; set; }
}

public class TypeStatisticDTO
{
    public int TotalCompletedOrders { get; set; }
    public int TotalCanceledOrders { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class MonthlyReportDTO
{
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal TourRevenue { get; set; }
    public int TourCompletedOrders { get; set; }
    public int TourCanceledOrders { get; set; }
    public decimal ComboRevenue { get; set; }
    public decimal ComboCompletedOrders { get; set; }
    public int ComboCanceledOrders { get; set; }
    public decimal AccommodationRevenue { get; set; }
    public int AccommodationCompletedOrders { get; set; }
    public int AccommodationCanceledOrders { get; set; }
}
