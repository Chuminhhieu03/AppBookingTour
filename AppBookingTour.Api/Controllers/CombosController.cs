using MediatR;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.Combos.CreateCombo;
using AppBookingTour.Application.Features.Combos.GetComboById;
using AppBookingTour.Application.Features.Combos.UpdateCombo;
using AppBookingTour.Application.Features.Combos.DeleteCombo;

namespace AppBookingTour.Api.Controllers;

[ApiController]
[Route("api/combos")]
public sealed class CombosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CombosController> _logger;

    public CombosController(IMediator mediator, ILogger<CombosController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> CreateCombo([FromBody] ComboRequestDTO requestBody)
    {
        try
        {
            var command = new CreateComboCommand(requestBody);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            _logger.LogInformation("Created new combo with ID: {ComboId}", result.Combo!.Id);
            return CreatedAtAction(nameof(GetComboById), new { id = result.Combo!.Id }, ApiResponse<object>.Ok(result.Combo!));
        }
        catch (ValidationException vex)
        {
            var messages = string.Join("; ", vex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            _logger.LogWarning(vex, "Validation failed for create combo: {Errors}", messages);
            return BadRequest(ApiResponse<object>.Fail(messages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating combo");
            return BadRequest(ApiResponse<object>.Fail("An error occurred while creating the combo."));
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetComboById(int id)
    {
        try
        {
            var query = new GetComboByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                if (result.ErrorMessage?.Contains("not found") == true)
                {
                    return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));
                }
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            _logger.LogInformation("Retrieved combo details for ID: {ComboId}", id);
            return Ok(ApiResponse<object>.Ok(result.Combo!));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving combo with ID {ComboId}", id);
            return BadRequest(ApiResponse<object>.Fail("An error occurred while retrieving the combo."));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateCombo(int id, [FromBody] ComboRequestDTO requestBody)
    {
        try
        {
            var command = new UpdateComboCommand(id, requestBody);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                if (result.ErrorMessage?.Contains("not found") == true)
                {
                    return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));
                }
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            _logger.LogInformation("Updated combo with ID {ComboId}", id);
            return Ok(new ApiResponse<object> { Success = true, Message = "Update combo successfully" });
        }
        catch (ValidationException vex)
        {
            var messages = string.Join("; ", vex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            _logger.LogWarning(vex, "Validation failed for update combo: {Errors}", messages);
            return BadRequest(ApiResponse<object>.Fail(messages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating combo with ID {ComboId}", id);
            return BadRequest(ApiResponse<object>.Fail("An error occurred while updating the combo."));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteCombo(int id)
    {
        try
        {
            var command = new DeleteComboCommand(id);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                if (result.ErrorMessage?.Contains("not found") == true)
                {
                    return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));
                }
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            _logger.LogInformation("Deleted combo with ID {ComboId}", id);
            return Ok(new ApiResponse<object> { Success = true, Message = "Delete combo successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting combo with ID {ComboId}", id);
            return BadRequest(ApiResponse<object>.Fail("An error occurred while deleting the combo."));
        }
    }
}