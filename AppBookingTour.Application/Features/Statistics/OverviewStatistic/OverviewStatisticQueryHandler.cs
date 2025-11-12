using AppBookingTour.Application.IRepositories;

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Statistics.OverviewStatistic;

public sealed class OverviewStatisticQueryHandler : IRequestHandler<OverviewStatisticQuery, OverviewStatisticDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<OverviewStatisticQueryHandler> _logger;
    private readonly IMemoryCache _cache;
    public OverviewStatisticQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<OverviewStatisticQueryHandler> logger,
        IMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }
    public async Task<OverviewStatisticDTO> Handle(OverviewStatisticQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling OverviewStatisticQuery for current month-to-date");

        var today = DateTime.Now.Date;

        var cacheKey = $"OverviewStats_{today:yyyy-MM-dd}";
        if (_cache.TryGetValue(cacheKey, out OverviewStatisticDTO cachedData))
        {
            _logger.LogInformation("Cache HIT: Get data from cache");
            return cachedData;
        }
        _logger.LogInformation("Cache MISS: Caculate OverviewStatisticQuery...");

        var currentYear = today.Year;
        var currentMonth = today.Month;

        var currentPeriodStart = new DateTime(currentYear, currentMonth, 1);
        var currentPeriodEnd = today;

        var previousPeriodStartDate = currentPeriodStart.AddMonths(-1);
        // Đảm bảo "cùng kỳ" tháng trước không vượt quá số ngày của tháng trước
        var daysInPreviousMonth = DateTime.DaysInMonth(previousPeriodStartDate.Year, previousPeriodStartDate.Month);
        var previousPeriodEndDay = Math.Min(today.Day, daysInPreviousMonth); // logic lấy min của số ngày tháng trước và ngày của tháng này
        var previousPeriodEnd = new DateTime(previousPeriodStartDate.Year, previousPeriodStartDate.Month, previousPeriodEndDay);

        var yearStart = new DateTime(currentYear, 1, 1);

        var overviewData = await _unitOfWork.Statistics.GetOverviewStatisticsAsync(
            currentPeriodStart,
            currentPeriodEnd,
            previousPeriodStartDate,
            previousPeriodEnd,
            yearStart,
            cancellationToken);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(30));

        _cache.Set(cacheKey, overviewData, cacheOptions);

        return overviewData;
    }
}