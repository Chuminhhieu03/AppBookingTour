
namespace AppBookingTour.Application.Features.TourCategories.UpdateTourCategory;

public class UpdateTourCategoryResponse
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }

    public static UpdateTourCategoryResponse Success() =>
        new() { IsSuccess = true, Message = "Update tour category successfully!" };

    public static UpdateTourCategoryResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, Message = errorMessage };
}