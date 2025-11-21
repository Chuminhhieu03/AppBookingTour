using MediatR;
using Microsoft.AspNetCore.Mvc;

using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.Tours.CreateTour;
using AppBookingTour.Application.Features.Tours.SearchTours;
using AppBookingTour.Application.Features.Tours.SearchToursForCustomer;
using AppBookingTour.Application.Features.Tours.GetTourById;
using AppBookingTour.Application.Features.Tours.GetFeaturedTours;
using AppBookingTour.Application.Features.Tours.UpdateTour;
using AppBookingTour.Application.Features.Tours.DeleteTour;

namespace AppBookingTour.Api.Controllers;

[ApiController]
[Route("api/tours")]
public sealed class ToursController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ToursController> _logger;

    public ToursController(IMediator mediator, ILogger<ToursController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> CreateTour([FromForm] TourCreateRequestDTO requestBody)
    {
        var command = new CreateTourCommand(requestBody);
        var result = await _mediator.Send(command);

        _logger.LogInformation("Created new tour with ID: {TourId}", result.Id);
        return Created("", ApiResponse<object>.Ok(result));
    }

    [HttpPost("search")]
    public async Task<ActionResult<ApiResponse<object>>> SearchTours([FromBody] SearchToursQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpPost("search-for-customer")]
    public async Task<ActionResult<ApiResponse<object>>> SearchToursForCustomer([FromBody] SearchToursForCustomerQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetTourById(int id)
    {
        var query = new GetTourByIdQuery(id);
        var result = await _mediator.Send(query);

        _logger.LogInformation("Retrieved tour details for ID: {TourId}", id);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateTour(int id, [FromForm] TourCreateRequestDTO requestBody)
    {
        var command = new UpdateTourCommand(id, requestBody);
        var result = await _mediator.Send(command);

        _logger.LogInformation("Updated tour with ID: {TourId}", id);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteTour(int id)
    {
        var command = new DeleteTourCommand(id);
        var result = await _mediator.Send(command);

        _logger.LogInformation("Deleted tour with ID: {TourId}", id);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Delete tour successfully"
        });
    }

    [HttpGet("featured")]
    public async Task<ActionResult<ApiResponse<List<FeaturedTourDTO>>>> GetFeaturedTours([FromQuery] int count = 6)
    {
        if (count <= 0 || count > 50)
        {
            return BadRequest(ApiResponse<List<FeaturedTourDTO>>.Fail("Số lượng phải từ 1 đến 50"));
        }

        var query = new GetFeaturedToursQuery(count);
        var result = await _mediator.Send(query);

        _logger.LogInformation("Retrieved {Count} featured tours", result.Count);
        return Ok(ApiResponse<List<FeaturedTourDTO>>.Ok(result));
    }
}
