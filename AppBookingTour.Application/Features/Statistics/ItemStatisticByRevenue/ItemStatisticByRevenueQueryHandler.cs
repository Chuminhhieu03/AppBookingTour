using AppBookingTour.Application.Features.Statistics.OverviewStatistic;
using AppBookingTour.Application.IRepositories;
using MediatR;
using Microsoft.Extensions.Logging;
using AppBookingTour.Application.Features.Tours.SearchTours;

namespace AppBookingTour.Application.Features.Statistics.ItemStatisticByRevenue;

public class ItemStatisticByRevenueQueryHandler : IRequestHandler<ItemStatisticByRevenueQuery, ItemStatisticByRevenueResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OverviewStatisticQueryHandler> _logger;
    public ItemStatisticByRevenueQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<OverviewStatisticQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<ItemStatisticByRevenueResponse> Handle(ItemStatisticByRevenueQuery request, CancellationToken cancellationToken)
    {
        var startDate = request.StartDate;
        var endDate = request.EndDate;
        var itemType = request.ItemType;
        var pageIndex = request.PageIndex > 0 ? request.PageIndex.Value : 1;
        var pageSize = request.PageSize > 0 ? request.PageSize.Value : 10;
        var isDesc = request.IsDesc ?? true;

        _logger.LogInformation("Handling ItemStatisticByRevenueQuery from {StartDate} to {EndDate} for ItemType: {ItemType}",
            startDate,
            endDate,
            itemType.ToString());

        var (items, totalCount, totalPages) = await _unitOfWork.Statistics.GetItemRevenueStatisticsAsync(
            startDate,
            endDate,
            itemType,
            pageIndex,
            pageSize,
            isDesc,
            cancellationToken);

        return new ItemStatisticByRevenueResponse
        {
            ItemTypeId = (int)request.ItemType,
            ItemTypeName = request.ItemType.ToString(),
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Items = items.ToList(),
            Meta = new PaginationMeta
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                Page = pageIndex,
                PageSize = pageSize
            }
        };
    }
}