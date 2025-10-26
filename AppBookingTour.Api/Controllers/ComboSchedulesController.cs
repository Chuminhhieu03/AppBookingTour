using MediatR;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.ComboSchedules.CreateComboSchedule;
using AppBookingTour.Application.Features.ComboSchedules.GetComboScheduleById;
using AppBookingTour.Application.Features.ComboSchedules.UpdateComboSchedule;
using AppBookingTour.Application.Features.ComboSchedules.DeleteComboSchedule;

namespace AppBookingTour.Api.Controllers;

[ApiController]
[Route("api/combo-schedules")]
public sealed class ComboSchedulesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ComboSchedulesController> _logger;

    public ComboSchedulesController(IMediator mediator, ILogger<ComboSchedulesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> CreateComboSchedule([FromBody] ComboScheduleRequestDTO requestBody)
    {
        try
        {
            var command = new CreateComboScheduleCommand(requestBody);
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            _logger.LogInformation("Created new combo schedule");
            return Created("", ApiResponse<object>.Ok(result.ComboSchedule!)); ;
        }
        catch (ValidationException vex)
        {
            var messages = string.Join("; ", vex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            _logger.LogWarning(vex, "Validation failed for create combo schedule: {Errors}", messages);
            return BadRequest(ApiResponse<object>.Fail(messages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating combo schedule");
            return BadRequest(ApiResponse<object>.Fail("An error occurred while creating the combo schedule"));
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetComboScheduleById(int id)
    {
        try
        {
            var query = new GetComboScheduleByIdQuery(id);
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                if (result.ErrorMessage?.Contains("not found") == true)
                {
                    return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));
                }
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }
            return Ok(ApiResponse<object>.Ok(result.ComboSchedule!));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving combo schedule");
            return BadRequest(ApiResponse<object>.Fail("An error occurred while retrieving the combo schedule"));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateComboSchedule(int id, [FromBody] ComboScheduleRequestDTO requestBody)
    {
        try
        {
            var command = new UpdateComboScheduleCommand(id, requestBody);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                if (result.ErrorMessage?.Contains("not found") == true)
                {
                    return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));
                }
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            _logger.LogInformation("Updated combo schedule with ID {Id}", id);
            return Ok(new ApiResponse<object> { Success = true, Message = "Update combo schedule successfully" });
        }
        catch (ValidationException vex)
        {
            var messages = string.Join("; ", vex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            _logger.LogWarning(vex, "Validation failed for update combo schedule: {Errors}", messages);
            return BadRequest(ApiResponse<object>.Fail(messages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating combo schedule");
            return BadRequest(ApiResponse<object>.Fail("An error occurred while updating the combo schedule"));
        }
    }


    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteComboSchedule(int id)
    {
        try
        {
            var command = new DeleteComboScheduleCommand(id);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                if (result.ErrorMessage?.Contains("not found") == true)
                {
                    return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));
                }
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            _logger.LogInformation("Deleted combo schedule with ID {Id}", id);
            return Ok(new ApiResponse<object> { Success = true, Message = "Delete combo schedule successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting combo schedule");
            return BadRequest(ApiResponse<object>.Fail("An error occurred while deleting the combo schedule"));
        }
    }
}