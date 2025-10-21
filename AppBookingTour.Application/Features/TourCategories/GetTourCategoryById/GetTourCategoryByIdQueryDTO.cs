
namespace AppBookingTour.Application.Features.TourCategories.GetTourCategoryById;

public class GetTourCategoryByIdResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public TourCategoryDTO? TourCategory { get; init; }

    public static GetTourCategoryByIdResponse Success(TourCategoryDTO tourCategory) =>
        new() { IsSuccess = true, TourCategory = tourCategory };

    public static GetTourCategoryByIdResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}

public class TourCategoryDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}