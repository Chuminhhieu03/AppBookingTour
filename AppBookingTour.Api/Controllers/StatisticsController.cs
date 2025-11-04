using AppBookingTour.Api.Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers;

[ApiController]
[Route("api/statistics")]
public sealed class StatisticsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<StatisticsController> _logger;
    public StatisticsController(IMediator mediator, ILogger<StatisticsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    //[HttpGet("overview")]
    //public async Task<ActionResult<ApiResponse<object>>> GetOverviewStatistic([FromQuery] int year)
    //{
    //}
}
