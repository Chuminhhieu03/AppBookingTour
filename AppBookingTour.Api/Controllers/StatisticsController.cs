using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.Statistics.OverviewStatistic;
using AppBookingTour.Application.Features.Statistics.ItemBookingCountDetail;
using AppBookingTour.Application.Features.Statistics.ItemRevenueDetail;
using AppBookingTour.Application.Features.Statistics.ItemStatisticByBookingCount;
using AppBookingTour.Application.Features.Statistics.ItemStatisticByRevenue;
using AppBookingTour.Domain.Enums;
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

    [HttpGet("overview")]
    public async Task<ActionResult<ApiResponse<object>>> GetOverviewStatistic()
    {
        var query = new OverviewStatisticQuery();
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("item-revenue")]
    public async Task<ActionResult<ApiResponse<object>>> GetItemStatisticByRevenue(
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate,
        [FromQuery] ItemType itemType)
    {
        var query = new ItemStatisticByRevenueQuery(startDate, endDate, itemType);
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("item-revenue-detail")]
    public async Task<ActionResult<ApiResponse<object>>> GetItemStatisticRevenueDetail(
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate,
        [FromQuery] ItemType itemType,
        [FromQuery] int itemId)
    {
        var query = new ItemRevenueDetailQuery(startDate, endDate, itemType, itemId);
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("item-booking-count")]
    public async Task<ActionResult<ApiResponse<object>>> GetItemStatisticByBookingCount(
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate,
        [FromQuery] ItemType itemType)
    {
        var query = new ItemStatisticByBookingCountQuery(startDate, endDate, itemType);
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("item-booking-count-detail")]
    public async Task<ActionResult<ApiResponse<object>>> GetItemStatisticBookingCountDetail(
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate,
        [FromQuery] ItemType itemType,
        [FromQuery] int itemId)
    {
        var query = new ItemBookingCountDetailQuery(startDate, endDate, itemType, itemId);
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<object>.Ok(result));
    }
}
