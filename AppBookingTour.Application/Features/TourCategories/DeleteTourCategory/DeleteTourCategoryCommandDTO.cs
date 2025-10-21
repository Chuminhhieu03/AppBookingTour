
namespace AppBookingTour.Application.Features.TourCategories.DeleteTourCategory;

public class DeleteTourCategoryResponse
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }

    public static DeleteTourCategoryResponse Success() =>
        new() { IsSuccess = true, Message = "Delete tour category successfully!" };

    public static DeleteTourCategoryResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, Message = errorMessage };
}