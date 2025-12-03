using AppBookingTour.Application.Features.Statistics.ItemBookingCountDetail;
using AppBookingTour.Application.Features.Statistics.ItemRevenueDetail;
using AppBookingTour.Application.Features.Statistics.ItemStatisticByBookingCount;
using AppBookingTour.Application.Features.Statistics.ItemStatisticByRevenue;
using AppBookingTour.Application.Features.Statistics.OverviewStatistic;
using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.IRepositories
{
    // Interface này không cần kế thừa IRepository<T> vì nó chuyên dụng
    public interface IStatisticsRepository
    {
        Task<(IEnumerable<ItemStatisticDTO> Items, int TotalCount, int TotalPages)> GetItemRevenueStatisticsAsync(
            DateOnly startDate,
            DateOnly endDate,
            ItemType itemType,
            int? pageIndex,
            int? pageSize,
            bool isDesc,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<ItemRevenueDetailDTO>> GetItemRevenueDetailAsync(
            DateOnly startDate,
            DateOnly endDate,
            ItemType itemType,
            int itemId,
            CancellationToken cancellationToken = default);

        Task<(IEnumerable<ItemStatisticByBookingCountDTO> Items, int TotalCount, int TotalPages)> GetItemBookingCountStatisticsAsync(
            DateOnly startDate,
            DateOnly endDate,
            ItemType itemType,
            int? pageIndex,
            int? pageSize,
            bool isDesc,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<ItemBookingCountDetailDTO>> GetItemBookingCountDetailAsync(
            DateOnly startDate,
            DateOnly endDate,
            ItemType itemType,
            int itemId,
            CancellationToken cancellationToken = default);

        Task<OverviewStatisticDTO> GetOverviewStatisticsAsync(
            DateTime currentPeriodStart,
            DateTime currentPeriodEnd,
            DateTime previousPeriodStart,
            DateTime previousPeriodEnd,
            DateTime yearStart,
            CancellationToken cancellationToken = default);
    }
}