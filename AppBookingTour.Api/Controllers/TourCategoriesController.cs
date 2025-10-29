using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.TourCategories.CreateTourCategory;
using AppBookingTour.Application.Features.TourCategories.DeleteTourCategory;
using AppBookingTour.Application.Features.TourCategories.GetTourCategoriesList;
using AppBookingTour.Application.Features.TourCategories.GetTourCategoryById;
using AppBookingTour.Application.Features.TourCategories.SearchTourCategory;
using AppBookingTour.Application.Features.TourCategories.UpdateTourCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers;

[ApiController]
[Route("api/tour-categories")]
public sealed class TourCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TourCategoriesController> _logger;

    public TourCategoriesController(IMediator mediator, ILogger<TourCategoriesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("search")]
    public async Task<ActionResult<ApiResponse<object>>> SearchTourCategories([FromBody] SearchTourCategoriesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("get-list")]
    public async Task<ActionResult<ApiResponse<object>>> GetTourCategoriesList()
    {
        var query = new GetTourCategoriesListQuery();
        var result = await _mediator.Send(query);

        _logger.LogInformation("Retrieved all tour categories");
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetTourCategoryById(int id)
    {
        var query = new GetTourCategoryByIdQuery(id);
        var result = await _mediator.Send(query);


        _logger.LogInformation("Retrieved tour category details for ID: {TourCategoryId}", id);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> CreateTourCategory([FromBody] TourCategoryRequestDTO requestBody)
    {
        var command = new CreateTourCategoryCommand(requestBody);
        var result = await _mediator.Send(command);

       
        _logger.LogInformation("Created new tour category with ID: {TourCategoryId}", result?.Id);
        return Created("", ApiResponse<object>.Ok(result!));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateTourCategory(int id, [FromBody] TourCategoryRequestDTO requestBody)
    {
        var command = new UpdateTourCategoryCommand(id, requestBody);
        var result = await _mediator.Send(command);

        _logger.LogInformation("Updated tour category with ID: {TourCategoryId}", id);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteTourCategory(int id)
    {
        var command = new DeleteTourCategoryCommand(id);
        await _mediator.Send(command);

        _logger.LogInformation("Deleted tour category with ID: {TourCategoryId}", id);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Delete tour category successfully"
        });
    }
}