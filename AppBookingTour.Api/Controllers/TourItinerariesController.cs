using MediatR;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;
using AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;
using AppBookingTour.Application.Features.TourItineraries.UpdateTourItinerary;
using AppBookingTour.Application.Features.TourItineraries.DeleteTourItinerary;

namespace AppBookingTour.Api.Controllers;

[ApiController]
[Route("api/tour-itineraries")]
public sealed class TourItinerariesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TourItinerariesController> _logger;

    public TourItinerariesController(IMediator mediator, ILogger<TourItinerariesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> CreateTourItinerary([FromBody] TourItineraryRequestDTO requestBody)
    {
        try
        {
            var command = new CreateTourItineraryCommand(requestBody);
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            _logger.LogInformation("Created new tour itinerary");
            return Created("", ApiResponse<object>.Ok(result.TourItinerary!));
        }
        catch (ValidationException vex)
        {
            var messages = string.Join("; ", vex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            _logger.LogWarning(vex, "Validation failed for create tour itinerary: {Errors}", messages);
            return BadRequest(ApiResponse<object>.Fail(messages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour itinerary");
            return BadRequest(ApiResponse<object>.Fail("An error occurred while creating the tour itinerary"));
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetTourItineraryById(int id)
    {
        try
        {
            var query = new GetTourItineraryByIdQuery(id);
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                if (result.ErrorMessage?.Contains("not found") == true)
                {
                    return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));
                }
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }
            return Ok(ApiResponse<object>.Ok(result.TourItinerary!));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tour itinerary");
            return BadRequest(ApiResponse<object>.Fail("An error occurred while retrieving the tour itinerary"));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateTourItinerary(int id, [FromBody] TourItineraryRequestDTO requestBody)
    {
        try
        {
            var command = new UpdateTourItineraryCommand(id, requestBody);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                if (result.ErrorMessage?.Contains("not found") == true)
                {
                    return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));
                }
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            _logger.LogInformation("Updated tour itinerary with ID {Id}", id);
            return Ok(new ApiResponse<object> { Success = true, Message = "Update tour itinerary successfully" });
        }
        catch (ValidationException vex)
        {
            var messages = string.Join("; ", vex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            _logger.LogWarning(vex, "Validation failed for update tour itinerary: {Errors}", messages);
            return BadRequest(ApiResponse<object>.Fail(messages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour itinerary");
            return BadRequest(ApiResponse<object>.Fail("An error occurred while updating the tour itinerary"));
        }
    }


    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteTourItinerary(int id)
    {
        try
        {
            var command = new DeleteTourItineraryCommand(id);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                if (result.ErrorMessage?.Contains("not found") == true)
                {
                    return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));
                }
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            _logger.LogInformation("Deleted tour itinerary with ID {Id}", id);
            return Ok(new ApiResponse<object> { Success = true, Message = "Delete tour itinerary successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour itinerary");
            return BadRequest(ApiResponse<object>.Fail("An error occurred while deleting the tour itinerary"));
        }
    }

}

