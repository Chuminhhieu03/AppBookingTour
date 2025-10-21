using AppBookingTour.Application.Features.TourCategories.GetTourCategoryById;

namespace AppBookingTour.Application.Features.TourCategories.CreateTourCategory;

public class CreateTourCategoryResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public TourCategoryDTO? TourCategory { get; init; }

    public static CreateTourCategoryResponse Success(TourCategoryDTO tourCategory) =>
        new() { IsSuccess = true, TourCategory = tourCategory };

    public static CreateTourCategoryResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}

public class TourCategoryRequestDTO
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? ParentCategoryId { get; set; }
    public string? ImageUrl { get; set; }
    public bool? IsActive { get; set; }
}