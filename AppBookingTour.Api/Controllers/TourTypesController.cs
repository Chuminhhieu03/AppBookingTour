using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.TourTypes.CreateTourType;
using AppBookingTour.Application.Features.TourTypes.DeleteTourType;
using AppBookingTour.Application.Features.TourTypes.GetTourTypeById;
using AppBookingTour.Application.Features.TourTypes.GetTourTypesList;
using AppBookingTour.Application.Features.TourTypes.UpdateTourType;
using FluentValidation;
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


    [HttpGet("get-list")]
    public async Task<ActionResult<ApiResponse<object>>> GetTourTypesList()
    {
        try
        {
            var query = new GetTourTypesListQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.Fail(result.Message!));
            }

            _logger.LogInformation("Retrieved all tour types");
            return Ok(ApiResponse<object>.Ok(result.TourTypes));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting tour types list");
            return BadRequest(ApiResponse<object>.Fail("An error occurred while retrieving tour types list"));
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetTourTypeById(int id)
    {
        try
        {
            var query = new GetTourTypeByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                if (result.ErrorMessage?.Contains("not found") == true)
                    return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));

                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            _logger.LogInformation("Retrieved tour type details for ID: {TourTypeId}", id);
            return Ok(ApiResponse<object>.Ok(result.TourType!));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting tour type details for ID: {TourTypeId}", id);
            return BadRequest(ApiResponse<object>.Fail("An error occurred while retrieving tour type details"));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> CreateTourType([FromBody] TourTypeRequestDTO requestBody)
    {
        try
        {
            var command = new CreateTourTypeCommand(requestBody);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse<object>.Fail(result.Message!));

            _logger.LogInformation("Created new tour type with ID: {TourTypeId}", result.TourType?.Id);

            return Created("", ApiResponse<object>.Ok(result.TourType!));
        }
        catch (ValidationException vex)
        {
            var messages = string.Join("; ", vex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            _logger.LogWarning(vex, "Validation failed for create tour type: {Errors}", messages);
            return BadRequest(ApiResponse<object>.Fail(messages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour type");
            return BadRequest(ApiResponse<object>.Fail("An error occurred while creating the tour type"));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateTourType(int id, [FromBody] TourTypeRequestDTO requestBody)
    {
        try
        {
            var command = new UpdateTourTypeCommand(id, requestBody);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                if (result.Message?.Contains("not found") == true)
                    return NotFound(ApiResponse<object>.Fail(result.Message!));

                return BadRequest(ApiResponse<object>.Fail(result.Message!));
            }

            _logger.LogInformation("Updated tour type with ID: {TourTypeId}", id);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Update tour type successfully"
            });
        }
        catch (ValidationException vex)
        {
            var messages = string.Join("; ", vex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            _logger.LogWarning(vex, "Validation failed for update tour type: {Errors}", messages);
            return BadRequest(ApiResponse<object>.Fail(messages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour type with ID: {TourTypeId}", id);
            return BadRequest(ApiResponse<object>.Fail("An error occurred while updating the tour type"));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteTourType(int id)
    {
        try
        {
            var command = new DeleteTourTypeCommand(id);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                if (result.Message?.Contains("not found") == true)
                    return NotFound(ApiResponse<object>.Fail(result.Message!));

                return BadRequest(ApiResponse<object>.Fail(result.Message!));
            }

            _logger.LogInformation("Deleted tour type with ID: {TourTypeId}", id);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Delete tour type successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour type with ID: {TourTypeId}", id);
            return BadRequest(ApiResponse<object>.Fail("An error occurred while deleting the tour type"));
        }
    }
}