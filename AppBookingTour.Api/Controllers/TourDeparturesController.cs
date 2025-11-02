using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.TourDepartures.CreateTourDeparture;
using AppBookingTour.Application.Features.TourDepartures.DeleteTourDeparture;
using AppBookingTour.Application.Features.TourDepartures.GetListTourDeparture;
using AppBookingTour.Application.Features.TourDepartures.GetTourDepartureById;
using AppBookingTour.Application.Features.TourDepartures.UpdateTourDeparture;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers;

[ApiController]
[Route("api/tour-departures")]
public sealed class TourDeparturesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TourDeparturesController> _logger;

    public TourDeparturesController(IMediator mediator, ILogger<TourDeparturesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> CreateTourDeparture([FromBody] TourDepartureRequestDTO requestBody)
    {
        var command = new CreateTourDepartureCommand(requestBody);
        var result = await _mediator.Send(command);

        _logger.LogInformation("Created new tour departure");
        return Created("", ApiResponse<object>.Ok(result));
    }

    [HttpGet("get-list/{tourId:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetTourDeparturesByTourId(int tourId)
    {
        var query = new GetTourDeparturesByTourIdQuery(tourId);
        var result = await _mediator.Send(query);

        _logger.LogInformation("Retrieved tour departures for Tour ID {TourId}", tourId);
        return Ok(ApiResponse<List<ListTourDepartureItem>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetTourDepartureById(int id)
    {
        var query = new GetTourDepartureByIdQuery(id);
        var result = await _mediator.Send(query);

        _logger.LogInformation("Retrieved tour departure with ID {TourDepartureId}", id);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateTourDeparture(int id, [FromBody] TourDepartureRequestDTO requestBody)
    {
        var command = new UpdateTourDepartureCommand(id, requestBody);
        var result = await _mediator.Send(command);

        _logger.LogInformation("Updated tour departure with ID {TourDepartureId}", id);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteTourDeparture(int id)
    {
        var command = new DeleteTourDepartureCommand(id);
        await _mediator.Send(command);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Delete tour departure successfully"
        });
    }
}
