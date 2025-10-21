using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.TourCategories.CreateTourCategory;
using AppBookingTour.Application.Features.TourCategories.DeleteTourCategory;
using AppBookingTour.Application.Features.TourCategories.GetTourCategoriesList;
using AppBookingTour.Application.Features.TourCategories.GetTourCategoryById;
using AppBookingTour.Application.Features.TourCategories.UpdateTourCategory;
using FluentValidation;
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

    [HttpGet("get-list")]
    public async Task<ActionResult<ApiResponse<object>>> GetTourCategoriesList()
    {
        try
        {
            var query = new GetTourCategoriesListQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.Fail(result.Message!));
            }

            _logger.LogInformation("Retrieved all tour categories");
            return Ok(ApiResponse<object>.Ok(result.TourCategories));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting tour categories list");
            return BadRequest(ApiResponse<object>.Fail("An error occurred while retrieving tour categories list"));
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetTourCategoryById(int id)
    {
        try
        {
            var query = new GetTourCategoryByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                if (result.ErrorMessage?.Contains("not found") == true)
                    return NotFound(ApiResponse<object>.Fail(result.ErrorMessage!));

                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            _logger.LogInformation("Retrieved tour category details for ID: {TourCategoryId}", id);
            return Ok(ApiResponse<object>.Ok(result.TourCategory!));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting tour category details for ID: {TourCategoryId}", id);
            return BadRequest(ApiResponse<object>.Fail("An error occurred while retrieving tour category details"));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> CreateTourCategory([FromBody] TourCategoryRequestDTO requestBody)
    {
        try
        {
            var command = new CreateTourCategoryCommand(requestBody);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));

            _logger.LogInformation("Created new tour category with ID: {TourCategoryId}", result.TourCategory?.Id);

            return Created("", ApiResponse<object>.Ok(result.TourCategory!));
        }
        catch (ValidationException vex)
        {
            var messages = string.Join("; ", vex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            _logger.LogWarning(vex, "Validation failed for create tour category: {Errors}", messages);
            return BadRequest(ApiResponse<object>.Fail(messages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour category");
            return BadRequest(ApiResponse<object>.Fail("An error occurred while creating the tour category"));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateTourCategory(int id, [FromBody] TourCategoryRequestDTO requestBody)
    {
        try
        {
            var command = new UpdateTourCategoryCommand(id, requestBody);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                if (result.Message?.Contains("not found") == true)
                    return NotFound(ApiResponse<object>.Fail(result.Message!));

                if (result.Message?.Contains("parent") == true)
                    return BadRequest(ApiResponse<object>.Fail(result.Message!));

                return BadRequest(ApiResponse<object>.Fail(result.Message!));
            }

            _logger.LogInformation("Updated tour category with ID: {TourCategoryId}", id);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Update tour category successfully"
            });
        }
        catch (ValidationException vex)
        {
            var messages = string.Join("; ", vex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            _logger.LogWarning(vex, "Validation failed for update tour category: {Errors}", messages);
            return BadRequest(ApiResponse<object>.Fail(messages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour category with ID: {TourCategoryId}", id);
            return BadRequest(ApiResponse<object>.Fail("An error occurred while updating the tour category"));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteTourCategory(int id)
    {
        try
        {
            var command = new DeleteTourCategoryCommand(id);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                if (result.Message?.Contains("not found") == true)
                    return NotFound(ApiResponse<object>.Fail(result.Message!));

                return BadRequest(ApiResponse<object>.Fail(result.Message!));
            }

            _logger.LogInformation("Deleted tour category with ID: {TourCategoryId}", id);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Delete tour category successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour category with ID: {TourCategoryId}", id);
            return BadRequest(ApiResponse<object>.Fail("An error occurred while deleting the tour category"));
        }
    }
}