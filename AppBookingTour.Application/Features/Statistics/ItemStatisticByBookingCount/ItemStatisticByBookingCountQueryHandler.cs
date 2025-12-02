using AppBookingTour.Application.IRepositories;
using MediatR;
using Microsoft.Extensions.Logging;
using AppBookingTour.Application.Features.Tours.SearchTours;

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
        var pageIndex = request.PageIndex > 0 ? request.PageIndex.Value : 1;
        var pageSize = request.PageSize > 0 ? request.PageSize.Value : 10;
        var isDesc = request.IsDesc ?? true;

        _logger.LogInformation("Handling ItemStatisticByBookingCountQuery from {StartDate} to {EndDate} for ItemType: {ItemType}", startDate, endDate, itemType);
        var (items, totalCount, totalPages) = await _unitOfWork.Statistics.GetItemBookingCountStatisticsAsync(
             startDate,
             endDate,
             itemType,
             pageIndex,
             pageSize,
             isDesc,
             cancellationToken);

        var response = new ItemStatisticByBookingCountResponse
        {
            ItemTypeId = (int)itemType,
            ItemTypeName = itemType.ToString(),
            StartDate = startDate,
            EndDate = endDate,
            Items = items.ToList(),
            Meta = new PaginationMeta
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                Page = pageIndex,
                PageSize = pageSize
            }
        };

        return response;
    }
}