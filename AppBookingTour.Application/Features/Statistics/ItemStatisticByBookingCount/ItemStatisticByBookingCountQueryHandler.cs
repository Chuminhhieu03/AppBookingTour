using AppBookingTour.Application.IRepositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Statistics.ItemStatisticByBookingCount;

public class ItemStatisticByBookingCountQueryHandler : IRequestHandler<ItemStatisticByBookingCountQuery, ItemStatisticByBookingCountResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ItemStatisticByBookingCountQueryHandler> _logger;

    public ItemStatisticByBookingCountQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<ItemStatisticByBookingCountQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ItemStatisticByBookingCountResponse> Handle(ItemStatisticByBookingCountQuery request, CancellationToken cancellationToken)
    {
        var endDate = request.EndDate;
        var startDate = request.StartDate;
        var itemType = request.ItemType;

        _logger.LogInformation("Handling ItemStatisticByBookingCountQuery from {StartDate} to {EndDate} for ItemType: {ItemType}", startDate, endDate, itemType);

        // Handler logic to get item statistics by booking count goes here

        return new ItemStatisticByBookingCountResponse();
    }
}