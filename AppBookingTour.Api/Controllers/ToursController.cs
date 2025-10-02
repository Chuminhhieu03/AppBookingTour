using AppBookingTour.Application.Features.Tours.GetToursList;
using AppBookingTour.Application.Features.Tours.GetTourById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AppBookingTour.Api.Controllers;

/// <summary>
/// Tours management controller following Clean Architecture
/// Uses MediatR with integrated DTOs in use cases
/// </summary>
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

    /// <summary>
    /// Get tours list with pagination and search
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    /// <param name="searchTerm">Search term for tour name/description</param>
    /// <param name="cityId">Filter by departure city ID</param>
    /// <param name="maxPrice">Maximum price filter</param>
    /// <returns>Paginated tours list with metadata</returns>
    [HttpGet]
    [OutputCache(Duration = 300)]
    [ProducesResponseType(typeof(GetToursListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetToursListResponse>> GetTours(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? cityId = null,
        [FromQuery] decimal? maxPrice = null)
    {
        try
        {
            var query = new GetToursListQuery(page, pageSize, searchTerm, cityId, maxPrice);
            var result = await _mediator.Send(query);
            
            _logger.LogInformation("Retrieved {TourCount} tours for page {Page} with search '{SearchTerm}'", 
                result.Tours.Count, page, searchTerm);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting tours list");
            return BadRequest(new { Success = false, Message = "An error occurred while retrieving tours" });
        }
    }

    /// <summary>
    /// Get tour details by ID with comprehensive information
    /// </summary>
    /// <param name="id">Tour ID</param>
    /// <returns>Detailed tour information including itinerary and departures</returns>
    [HttpGet("{id:int}")]
    [OutputCache(Duration = 300)]
    [ProducesResponseType(typeof(TourDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TourDetailDto>> GetTourById(int id)
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

    /// <summary>
    /// Get popular tours (top rated and most booked)
    /// </summary>
    /// <param name="limit">Number of tours to return (default: 10)</param>
    /// <returns>List of popular tours</returns>
    [HttpGet("popular")]
    [OutputCache(Duration = 600)] // Cache for 10 minutes
    [ProducesResponseType(typeof(GetToursListResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetToursListResponse>> GetPopularTours([FromQuery] int limit = 10)
    {
        try
        {
            // Use existing query with enhanced logic for popular tours
            var query = new GetToursListQuery(1, limit);
            var result = await _mediator.Send(query);
            
            // Sort by rating and total bookings (this could be enhanced in the handler)
            result.Tours = result.Tours
                .OrderByDescending(t => t.Rating)
                .ThenByDescending(t => t.TotalBookings)
                .Take(limit)
                .ToList();

            _logger.LogInformation("Retrieved {TourCount} popular tours", result.Tours.Count);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting popular tours");
            return BadRequest(new { Success = false, Message = "An error occurred while retrieving popular tours" });
        }
    }

    /// <summary>
    /// Search tours by various criteria
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <param name="cityId">Departure city ID</param>
    /// <param name="maxPrice">Maximum price</param>
    /// <param name="minRating">Minimum rating</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Filtered tours list</returns>
    [HttpGet("search")]
    [OutputCache(Duration = 180)] // Cache for 3 minutes
    [ProducesResponseType(typeof(GetToursListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetToursListResponse>> SearchTours(
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? cityId = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] decimal? minRating = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var query = new GetToursListQuery(page, pageSize, searchTerm, cityId, maxPrice);
            var result = await _mediator.Send(query);
            
            // Apply rating filter if provided (could be moved to use case)
            if (minRating.HasValue)
            {
                result.Tours = result.Tours
                    .Where(t => t.Rating >= minRating.Value)
                    .ToList();
                result.TotalCount = result.Tours.Count;
                result.TotalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize);
            }

            _logger.LogInformation("Search returned {TourCount} tours for criteria: SearchTerm='{SearchTerm}', CityId={CityId}, MaxPrice={MaxPrice}, MinRating={MinRating}", 
                result.Tours.Count, searchTerm, cityId, maxPrice, minRating);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching tours");
            return BadRequest(new { Success = false, Message = "An error occurred while searching tours" });
        }
    }

    /// <summary>
    /// Get tours statistics (Admin only)
    /// </summary>
    /// <returns>Tours statistics</returns>
    [HttpGet("statistics")]
    [Authorize(Policy = "AdminPolicy")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> GetToursStatistics()
    {
        try
        {
            // Get all tours for statistics
            var query = new GetToursListQuery(1, int.MaxValue);
            var result = await _mediator.Send(query);

            var statistics = new
            {
                TotalTours = result.TotalCount,
                ActiveTours = result.Tours.Count(t => t.IsActive),
                AverageRating = result.Tours.Where(t => t.Rating > 0).Average(t => t.Rating),
                TotalBookings = result.Tours.Sum(t => t.TotalBookings),
                PopularTours = result.Tours
                    .OrderByDescending(t => t.TotalBookings)
                    .Take(5)
                    .Select(t => new { t.Id, t.Name, t.TotalBookings })
                    .ToList(),
                RecentTours = result.Tours
                    .OrderByDescending(t => t.CreatedAt)
                    .Take(5)
                    .Select(t => new { t.Id, t.Name, t.CreatedAt })
                    .ToList()
            };

            _logger.LogInformation("Generated tours statistics for admin");
            return Ok(statistics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating tours statistics");
            return BadRequest(new { Success = false, Message = "An error occurred while generating statistics" });
        }
    }
}