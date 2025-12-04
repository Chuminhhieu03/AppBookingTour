using MediatR;
using Microsoft.AspNetCore.Mvc;

using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;
using AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;
using AppBookingTour.Application.Features.TourItineraries.GetListTourItinerary;
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

    [HttpGet("get-list/{tourId:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetTourItinerariesByTourId(int tourId)
    {
        var query = new GetTourItinerariesByTourIdQuery(tourId);
        var result = await _mediator.Send(query);

        _logger.LogInformation("Retrieved tour itineraries for Tour ID {TourId}", tourId);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpPost("{tourId:int}")]
    public async Task<ActionResult<ApiResponse<object>>> CreateTourItinerary(int tourId, [FromBody] TourItineraryRequestDTO requestBody)
    {
        var command = new CreateTourItineraryCommand(tourId, requestBody);
        var result = await _mediator.Send(command);

        _logger.LogInformation("Created new tour itinerary");
        return Created("", ApiResponse<object>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetTourItineraryById(int id)
    {
        var query = new GetTourItineraryByIdQuery(id);
        var result = await _mediator.Send(query);
       
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateTourItinerary(int id, [FromBody] TourItineraryRequestDTO requestBody)
    {
        var command = new UpdateTourItineraryCommand(id, requestBody);
        var result = await _mediator.Send(command);

        _logger.LogInformation("Updated tour itinerary with ID {Id}", id);
        return Ok(ApiResponse<object>.Ok(result));
    }


    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteTourItinerary(int id)
    {
        var command = new DeleteTourItineraryCommand(id);
        await _mediator.Send(command);

        _logger.LogInformation("Deleted tour itinerary with ID {Id}", id);
        return Ok(new ApiResponse<object> { Success = true, Message = "Delete tour itinerary successfully" });
    }

}

