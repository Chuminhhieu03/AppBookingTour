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
        var endDate = request.EndDate;
        var startDate = request.StartDate;
        var itemType = request.ItemType;
        _logger.LogInformation("Handling ItemStatisticByRevenueQuery from {StartDate} to {EndDate} for ItemType: {ItemType}", startDate, endDate, itemType);
        // Handler logic to get item statistics by revenue goes here

        return new ItemStatisticByRevenueResponse();
    }
}
