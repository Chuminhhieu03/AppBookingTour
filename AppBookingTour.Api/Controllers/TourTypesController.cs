using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.TourTypes.CreateTourType;
using AppBookingTour.Application.Features.TourTypes.DeleteTourType;
using AppBookingTour.Application.Features.TourTypes.GetTourTypeById;
using AppBookingTour.Application.Features.TourTypes.GetTourTypesList;
using AppBookingTour.Application.Features.TourTypes.SearchTourType;
using AppBookingTour.Application.Features.TourTypes.UpdateTourType;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers;

[ApiController]
[Route("api/tour-types")]
public sealed class TourTypesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TourTypesController> _logger;

    public TourTypesController(IMediator mediator, ILogger<TourTypesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("search")]
    public async Task<ActionResult<ApiResponse<object>>> SearchTourTypes([FromBody] SearchTourTypesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("get-list")]
    public async Task<ActionResult<ApiResponse<object>>> GetTourTypesList()
    {
        var query = new GetTourTypesListQuery();
        var result = await _mediator.Send(query);

        _logger.LogInformation("Retrieved all tour types");
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetTourTypeById(int id)
    {
        var query = new GetTourTypeByIdQuery(id);
        var result = await _mediator.Send(query);

        _logger.LogInformation("Retrieved tour type details for ID: {TourTypeId}", id);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> CreateTourType([FromBody] TourTypeRequestDTO requestBody)
    {
        var command = new CreateTourTypeCommand(requestBody);
        var result = await _mediator.Send(command);

        _logger.LogInformation("Created new tour type with ID: {TourTypeId}", result?.Id);
        return Created("", ApiResponse<object>.Ok(result!));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateTourType(int id, [FromBody] TourTypeRequestDTO requestBody)
    {
        var command = new UpdateTourTypeCommand(id, requestBody);
        var result = await _mediator.Send(command);

        _logger.LogInformation("Updated tour type with ID: {TourTypeId}", id);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteTourType(int id)
    {
        var command = new DeleteTourTypeCommand(id);
        await _mediator.Send(command);

        _logger.LogInformation("Deleted tour type with ID: {TourTypeId}", id);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Delete tour type successfully"
        });
    }
}