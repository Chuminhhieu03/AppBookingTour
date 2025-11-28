using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.Reviews.CreateReview;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers;

/// <summary>
/// Review management controller
/// </summary>
[ApiController]
[Route("api/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ReviewsController> _logger;

    public ReviewsController(IMediator mediator, ILogger<ReviewsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Create a new review for a completed booking
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CreateReviewResponse>>> CreateReview(
        [FromBody] CreateReviewRequest request)
    {
        var command = new CreateReviewCommand(request);
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(ApiResponse<CreateReviewResponse>.Fail(result.Message));
        }

        _logger.LogInformation("Review created successfully for booking {BookingId} by user {UserId}", 
            request.BookingId, request.UserId);
        
        return Ok(ApiResponse<CreateReviewResponse>.Ok(result));
    }
}
