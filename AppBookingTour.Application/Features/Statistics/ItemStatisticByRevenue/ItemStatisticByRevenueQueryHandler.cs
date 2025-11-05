using AppBookingTour.Application.Features.Statistics.OverviewStatistic;
using AppBookingTour.Application.IRepositories;
using MediatR;
using Microsoft.Extensions.Logging;

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
        _logger.LogInformation("Handling ItemStatisticByRevenueQuery from {StartDate} to {EndDate} for ItemType: {ItemType}",
            request.StartDate,
            request.EndDate,
            request.ItemType.ToString());

        var statisticItems = await _unitOfWork.Statistics.GetItemRevenueStatisticsAsync(
            request.StartDate,
            request.EndDate,
            request.ItemType,
            cancellationToken);

        return new ItemStatisticByRevenueResponse
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Items = statisticItems.ToList()
        };
    }
}