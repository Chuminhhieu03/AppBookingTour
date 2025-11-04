using AppBookingTour.Application.IRepositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Statistics.ItemBookingCountDetail;

public class ItemBookingCountDetailQueryHandler : IRequestHandler<ItemBookingCountDetailQuery, ItemBookingCountDetailResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ItemBookingCountDetailQueryHandler> _logger;

    public ItemBookingCountDetailQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<ItemBookingCountDetailQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ItemBookingCountDetailResponse> Handle(ItemBookingCountDetailQuery request, CancellationToken cancellationToken)
    {
        var endDate = request.EndDate;
        var startDate = request.StartDate;
        var itemType = request.ItemType;
        var itemId = request.ItemId;

        _logger.LogInformation("Handling ItemBookingCountDetailQuery from {StartDate} to {EndDate} for ItemType: {ItemType}, ItemId: {ItemId}", startDate, endDate, itemType, itemId);

        // Handler logic to get item booking count details goes here

        return new ItemBookingCountDetailResponse();
    }
}