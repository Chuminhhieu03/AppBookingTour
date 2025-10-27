using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.Cities.GetCityById;
using AppBookingTour.Application.Features.Cities.GetListCity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers;

[ApiController]
[Route("api/cities")]
public sealed class CitiesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CitiesController> _logger;

    public CitiesController(IMediator mediator, ILogger<CitiesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("get-list")]
    public async Task<ActionResult<ApiResponse<object>>> GetCitiesList()
    {
        var query = new GetListCityQuery();
        var result = await _mediator.Send(query);

        _logger.LogInformation("Retrieved all cities");
        return Ok(ApiResponse<object>.Ok(result.Cities));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetCityById(int id)
    {
        try
        {
            var query = new GetCityByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                if (result.ErrorMessage?.Contains("not found") == true)
                    return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));

                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            _logger.LogInformation("Retrieved city details for ID: {CityId}", id);
            return Ok(ApiResponse<object>.Ok(result.City!));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting city details for ID: {CityId}", id);
            return BadRequest(ApiResponse<object>.Fail("An error occurred while retrieving city details"));
        }
    }
}