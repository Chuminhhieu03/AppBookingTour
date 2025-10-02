using AppBookingTour.Application.IRepositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Tours.GetToursList;

/// <summary>
/// Get Tours List use case - Query, DTOs, Validator, Handler in one place
/// Following Clean Architecture with integrated DTOs and new repository structure
/// </summary>

#region Query & DTOs
public record GetToursListQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    int? CityId = null,
    decimal? MaxPrice = null
) : IRequest<GetToursListResponse>;

// Response DTOs - integrated in use case
public class GetToursListResponse
{
    public List<TourListItem> Tours { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}

public class TourListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public decimal BasePriceAdult { get; set; }
    public int DurationDays { get; set; }
    public decimal Rating { get; set; }
    public string DepartureCityName { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public int TotalBookings { get; set; }
    public string Status { get; set; } = null!;
    public string? TypeName { get; set; }
    public string? CategoryName { get; set; }
}
#endregion

#region Validator
public class GetToursListQueryValidator : AbstractValidator<GetToursListQuery>
{
    public GetToursListQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100");

        RuleFor(x => x.MaxPrice)
            .GreaterThan(0).WithMessage("Max price must be greater than 0")
            .When(x => x.MaxPrice.HasValue);

        RuleFor(x => x.SearchTerm)
            .MaximumLength(100).WithMessage("Search term must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.SearchTerm));
    }
}
#endregion

#region Handler
public sealed class GetToursListQueryHandler : IRequestHandler<GetToursListQuery, GetToursListResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetToursListQueryHandler> _logger;

    public GetToursListQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetToursListQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<GetToursListResponse> Handle(GetToursListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing GetToursList query: Page={Page}, PageSize={PageSize}, SearchTerm={SearchTerm}", 
            request.Page, request.PageSize, request.SearchTerm);

        try
        {
            IEnumerable<Domain.Entities.Tour> tours;
            int totalCount;

            // Use specific search method if search criteria provided
            if (!string.IsNullOrEmpty(request.SearchTerm) || request.CityId.HasValue || request.MaxPrice.HasValue)
            {
                var searchResults = await _unitOfWork.Tours.SearchToursAsync(
                    request.SearchTerm ?? string.Empty, 
                    request.CityId, 
                    request.MaxPrice);
                
                tours = searchResults.Skip((request.Page - 1) * request.PageSize)
                                   .Take(request.PageSize);
                totalCount = searchResults.Count();
            }
            else
            {
                // Use pagination method for general listing
                var (pagedTours, count) = await _unitOfWork.Tours.GetPagedAsync(
                    page: request.Page,
                    pageSize: request.PageSize,
                    predicate: t => t.IsActive,
                    orderBy: t => t.CreatedAt,
                    descending: true,
                    cancellationToken: cancellationToken);

                tours = pagedTours;
                totalCount = count;
            }

            var tourListItems = tours.Select(tour => new TourListItem
            {
                Id = tour.Id,
                Name = tour.Name,
                Code = tour.Code,
                BasePriceAdult = tour.BasePriceAdult,
                DurationDays = tour.DurationDays,
                Rating = tour.Rating,
                DepartureCityName = tour.DepartureCity?.Name ?? "Unknown",
                ImageUrl = ExtractFirstImageUrl(tour.ImageGallery),
                IsActive = tour.IsActive,
                CreatedAt = tour.CreatedAt,
                TotalBookings = tour.TotalBookings,
                Status = tour.Status.ToString(),
                TypeName = tour.Type?.Name,
                CategoryName = tour.Category?.Name
            }).ToList();

            var response = new GetToursListResponse
            {
                Tours = tourListItems,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
            };

            _logger.LogInformation("Successfully retrieved {TourCount} tours out of {TotalCount} total tours", 
                tours.Count(), totalCount);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing GetToursList query");
            throw;
        }
    }

    private static string? ExtractFirstImageUrl(string? imageGallery)
    {
        if (string.IsNullOrEmpty(imageGallery))
            return null;

        try
        {
            // Simple JSON parsing for first image URL
            // In production, use proper JSON deserializer
            var firstImage = imageGallery.Split(',').FirstOrDefault()?.Trim(' ', '"', '[', ']');
            return firstImage;
        }
        catch
        {
            return null;
        }
    }
}
#endregion