using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
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

        var (itemCode, itemName) = await GetItemNameAndCode(itemType, itemId, cancellationToken);

        if (string.IsNullOrEmpty(itemName))
        {
            _logger.LogWarning("Parent item not found with ItemType: {ItemType}, ItemId: {ItemId}", itemType, itemId);
            throw new KeyNotFoundException($"Không tìm thấy đối tượng với Id={itemId} và Type={itemType}");
        }

        var details = await _unitOfWork.Statistics.GetItemBookingCountDetailAsync(
            startDate,
            endDate,
            itemType,
            itemId,
            cancellationToken);

        var response = new ItemBookingCountDetailResponse
        {
            StartDate = startDate,
            EndDate = endDate,
            ItemCode = itemCode,
            ItemName = itemName,
            ItemDetails = details.ToList()
        };

        return response;
    }

    private async Task<(string ItemCode, string ItemName)> GetItemNameAndCode(ItemType itemType, int itemId, CancellationToken cancellationToken)
    {
        switch (itemType)
        {
            case ItemType.Tour:
                var tour = await _unitOfWork.Tours.GetByIdAsync(itemId, cancellationToken);
                return (tour?.Code ?? string.Empty, tour?.Name ?? string.Empty);

            case ItemType.Combo:
                var combo = await _unitOfWork.Repository<Combo>().GetByIdAsync(itemId, cancellationToken);
                return (combo?.Code ?? string.Empty, combo?.Name ?? string.Empty);

            case ItemType.Accommodation:
                var accom = await _unitOfWork.Accommodations.GetByIdAsync(itemId, cancellationToken);
                return (accom?.Code ?? string.Empty, accom?.Name ?? string.Empty);

            default:
                return (string.Empty, string.Empty);
        }
    }
}