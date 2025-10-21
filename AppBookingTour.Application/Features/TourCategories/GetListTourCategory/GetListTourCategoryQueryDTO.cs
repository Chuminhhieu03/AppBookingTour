using AppBookingTour.Application.Features.TourCategories.GetTourCategoryById;

namespace AppBookingTour.Application.Features.TourCategories.GetTourCategoriesList;

public class GetTourCategoriesListResponse
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }
    public List<TourCategoryDTO> TourCategories { get; init; } = new();

    public static GetTourCategoriesListResponse Success(List<TourCategoryDTO> tourCategories) =>
        new() { IsSuccess = true, TourCategories = tourCategories, Message = "Retrieved tour categories successfully." };

    public static GetTourCategoriesListResponse Failed(string message) =>
        new() { IsSuccess = false, Message = message, TourCategories = new List<TourCategoryDTO>() };
}