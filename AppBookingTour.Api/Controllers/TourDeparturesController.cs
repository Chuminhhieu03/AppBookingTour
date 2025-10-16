using MediatR;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.TourDepartures.CreateTourDeparture;
using AppBookingTour.Application.Features.TourDepartures.GetTourDepartureById;
using AppBookingTour.Application.Features.TourDepartures.UpdateTourDeparture;
using AppBookingTour.Application.Features.TourDepartures.DeleteTourDeparture;

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
        try
        {
            var command = new CreateTourDepartureCommand(requestBody);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));

            _logger.LogInformation("Created new tour departure");
            return Created("", ApiResponse<object>.Ok(result.TourDeparture!));
        }
        catch (ValidationException vex)
        {
            var messages = string.Join("; ", vex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            _logger.LogWarning(vex, "Validation failed for create tour departure: {Errors}", messages);
            return BadRequest(ApiResponse<object>.Fail(messages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour departure");
            return BadRequest(ApiResponse<object>.Fail("An error occurred while creating the tour departure"));
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetTourDepartureById(int id)
    {
        try
        {
            var query = new GetTourDepartureByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                if (result.ErrorMessage?.Contains("not found") == true)
                    return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));

                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            return Ok(ApiResponse<object>.Ok(result.TourDeparture!));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tour departure with ID {Id}", id);
            return BadRequest(ApiResponse<object>.Fail("An error occurred while retrieving the tour departure"));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateTourDeparture(int id, [FromBody] TourDepartureRequestDTO requestBody)
    {
        try
        {
            var command = new UpdateTourDepartureCommand(id, requestBody);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                if (result.ErrorMessage?.Contains("not found") == true)
                    return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));

                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            _logger.LogInformation("Updated tour departure with ID {TourDepartureId}", id);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Update tour departure successfully"
            });
        }
        catch (ValidationException vex)
        {
            var messages = string.Join("; ", vex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            _logger.LogWarning(vex, "Validation failed for update tour departure: {Errors}", messages);
            return BadRequest(ApiResponse<object>.Fail(messages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour departure with ID {TourDepartureId}", id);
            return BadRequest(ApiResponse<object>.Fail("An error occurred while updating the tour departure"));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteTourDeparture(int id)
    {
        try
        {
            var command = new DeleteTourDepartureCommand(id);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                if (result.ErrorMessage?.Contains("not found") == true)
                    return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));

                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            _logger.LogInformation("Deleted tour departure with ID {TourDepartureId}", id);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Delete tour departure successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour departure with ID {TourDepartureId}", id);
            return BadRequest(ApiResponse<object>.Fail("An error occurred while deleting the tour departure"));
        }
    }
}
