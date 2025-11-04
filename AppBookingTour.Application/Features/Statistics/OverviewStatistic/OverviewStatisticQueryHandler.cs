using AppBookingTour.Application.IRepositories;

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Statistics.OverviewStatistic;

public sealed class OverviewStatisticQueryHandler : IRequestHandler<OverviewStatisticQuery, OverviewStatisticDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<OverviewStatisticQueryHandler> _logger;
    public OverviewStatisticQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<OverviewStatisticQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<OverviewStatisticDTO> Handle(OverviewStatisticQuery request, CancellationToken cancellationToken)
    {
        var year = request.year;
        var month = request.month;
        _logger.LogInformation("Handling OverviewStatisticQuery for Year: {Year}, Month: {Month}", year, month);
        // Handler logic to get overview statistics goes here
        return new OverviewStatisticDTO();
    }
}