using MediatR;
using Microsoft.AspNetCore.Mvc;
using AppBookingTour.Api.Contracts.Responses;

using AppBookingTour.Application.Features.Tours.CreateTour;
using AppBookingTour.Application.Features.Tours.GetTourById;
using AppBookingTour.Application.Features.Tours.UpdateTour;
using AppBookingTour.Application.Features.Tours.DeleteTour;

namespace AppBookingTour.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<ActionResult<ApiResponse<object>>> CreateTour([FromBody] TourRequestDTO requestBody)
    {
        try
        {
            var command = new CreateTourCommand(requestBody);
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(new { Success = false, Message = result.ErrorMessage });
            }

            _logger.LogInformation("Created new tour with ID: {TourId}", result.Tour?.Id);
            return Created("", ApiResponse<object>.Ok(result.Tour!));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour");
            return BadRequest(new { Success = false, Message = "An error occurred while creating the tour" });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetTourById(int id)
    {
        try
        {
            var query = new GetTourByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                if (result.ErrorMessage?.Contains("not found") == true)
                {
                    return NotFound(new { Success = false, Message = result.ErrorMessage });
                }

                return BadRequest(new { Success = false, Message = result.ErrorMessage });
            }

            _logger.LogInformation("Retrieved tour details for ID: {TourId}", id);
            return Ok(result.Tour);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting tour details for ID: {TourId}", id);
            return BadRequest(new { Success = false, Message = "An error occurred while retrieving tour details" });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateTour(int id, [FromBody] TourRequestDTO requestBody)
    {
        try
        {
            var command = new UpdateTourCommand(id, requestBody);
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {
                if (result.Message?.Contains("not found") == true)
                {
                    return NotFound(new { Success = false, Message = result.Message! });
                }
                return BadRequest(new { Success = false, Message = result.Message! });
            }

            _logger.LogInformation("Updated tour with ID: {TourId}", id);

            return Ok(new ApiResponse<object> { Success = true, Message = "Update tour successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour with ID: {TourId}", id);
            return BadRequest(new { Success = false, Message = "An error occurred while updating the tour" });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteTour(int id)
    {
        try
        {
            var command = new DeleteTourCommand(id);
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {
                if (result.Message?.Contains("not found") == true)
                {
                    return NotFound(new { Success = false, Message = result.Message! });
                }
                return BadRequest(new { Success = false, Message = result.Message! });
            }
            _logger.LogInformation("Deleted tour with ID: {TourId}", id);
            return Ok(new ApiResponse<object> { Success = true, Message = "Delete tour successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour with ID: {TourId}", id);
            return BadRequest(new { Success = false, Message = "An error occurred while deleting the tour" });
        }
    }

}