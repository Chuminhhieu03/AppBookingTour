
namespace AppBookingTour.Application.Features.Statistics.OverviewStatistic;

public class OverviewStatisticDTO
{
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal PreviousMonthRevenue { get; set; }
    public double GrowthRate { get; set; }
    public int TotalBookings { get; set; }
    public int CompletedBookings { get; set; }
    public int CanceledBookings { get; set; }
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
    public int TotalCompletedBooings { get; set; }
    public int TotalCanceledBookings { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class MonthlyReportDTO
{
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal TourRevenue { get; set; }
    public int TourCompletedBookings { get; set; }
    public decimal ComboRevenue { get; set; }
    public decimal ComboCompletedBookings { get; set; }
    public decimal AccommodationRevenue { get; set; }
    public int AccommodationCompletedBookings { get; set; }
}
