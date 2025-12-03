using AppBookingTour.Application.Features.Statistics.ItemStatisticByRevenue;
using AppBookingTour.Application.Features.Statistics.ItemStatisticByBookingCount;

namespace AppBookingTour.Application.IServices;

public interface IExcelExportService
{
    byte[] ExportRevenueStatistics(List<ItemStatisticDTO> data, DateOnly startDate, DateOnly endDate, string itemTypeName);

    byte[] ExportBookingCountStatistics(List<ItemStatisticByBookingCountDTO> data, DateOnly startDate, DateOnly endDate, string itemTypeName);
}