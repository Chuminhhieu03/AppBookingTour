using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.SystemParameters.CreateSystemParameter;
using AppBookingTour.Application.Features.SystemParameters.DeleteSystemParameter;
using AppBookingTour.Application.Features.SystemParameters.GetSystemParameterById;
using AppBookingTour.Application.Features.SystemParameters.UpdateSystemParameter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers
{
    [ApiController]
    [Route("api/system-parameters")]
    public sealed class SystemParametersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SystemParametersController> _logger;

        public SystemParametersController(IMediator mediator, ILogger<SystemParametersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> CreateSystemParameter([FromBody] SystemParameterRequestDTO requestBody)
        {
            try
            {
                var command = new CreateSystemParameterCommand(requestBody);
                var result = await _mediator.Send(command);

                if (!result.IsSuccess)
                {
                    return BadRequest(ApiResponse<object>.Fail(result.Message));
                }

                _logger.LogInformation("Created new system parameter with ID: {Id}", result.SystemParameter?.Id);
                return Created("", ApiResponse<object>.Ok(result.SystemParameter!));
            }
            catch (ValidationException vex)
            {
                var messages = string.Join("; ", vex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
                _logger.LogWarning(vex, "Validation failed for create system parameter: {Errors}", messages);
                return BadRequest(ApiResponse<object>.Fail(messages));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating system parameter");
                return BadRequest(ApiResponse<object>.Fail("An error occurred while creating the system parameter."));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<object>>> GetSystemParameterById(int id)
        {
            try
            {
                var query = new GetSystemParameterByIdQuery(id);
                var result = await _mediator.Send(query);

                if (!result.IsSuccess)
                {
                    if (result.ErrorMessage?.Contains("not found") == true)
                        return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));

                    return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
                }

                _logger.LogInformation("Retrieved system parameter details for ID: {Id}", id);
                return Ok(ApiResponse<object>.Ok(result.SystemParameter!));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting system parameter details for ID: {Id}", id);
                return BadRequest(ApiResponse<object>.Fail("An error occurred while retrieving system parameter details."));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateSystemParameter(int id, [FromBody] SystemParameterRequestDTO requestBody)
        {
            try
            {
                var command = new UpdateSystemParameterCommand(id, requestBody);
                var result = await _mediator.Send(command);

                if (!result.IsSuccess)
                {
                    if (result.Message?.Contains("not found") == true)
                        return NotFound(ApiResponse<object>.Fail(result.Message!));

                    return BadRequest(ApiResponse<object>.Fail(result.Message!));
                }

                _logger.LogInformation("Updated system parameter with ID: {Id}", id);
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = result.Message ?? "Update system parameter successfully"
                });
            }
            catch (ValidationException vex)
            {
                var messages = string.Join("; ", vex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
                _logger.LogWarning(vex, "Validation failed for update system parameter: {Errors}", messages);
                return BadRequest(ApiResponse<object>.Fail(messages));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating system parameter with ID: {Id}", id);
                return BadRequest(ApiResponse<object>.Fail("An error occurred while updating the system parameter."));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteSystemParameter(int id)
        {
            try
            {
                var command = new DeleteSystemParameterCommand(id);
                var result = await _mediator.Send(command);

                if (!result.IsSuccess)
                {
                    if (result.Message?.Contains("not found") == true)
                        return NotFound(ApiResponse<object>.Fail(result.Message!));

                    return BadRequest(ApiResponse<object>.Fail(result.Message!));
                }

                _logger.LogInformation("Deleted system parameter with ID: {Id}", id);
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message ="Delete system parameter successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting system parameter with ID: {Id}", id);
                return BadRequest(ApiResponse<object>.Fail("An error occurred while deleting the system parameter."));
            }
        }
    }
}