using AppBookingTour.Application.IRepositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Statistics.ItemRevenueDetail;

public class ItemRevenueDetailQueryHandler : IRequestHandler<ItemRevenueDetailQuery, ItemRevenueDetailResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ItemRevenueDetailQueryHandler> _logger;
    public ItemRevenueDetailQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<ItemRevenueDetailQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<ItemRevenueDetailResponse> Handle(ItemRevenueDetailQuery request, CancellationToken cancellationToken)
    {
        var endDate = request.EndDate;
        var startDate = request.StartDate;
        var itemType = request.ItemType;
        var itemId = request.ItemId;
        _logger.LogInformation("Handling ItemRevenueDetailQuery from {StartDate} to {EndDate} for ItemType: {ItemType}, ItemId: {ItemId}", startDate, endDate, itemType, itemId);
        // Handler logic to get item revenue details goes here
        return new ItemRevenueDetailResponse();
    }
}
