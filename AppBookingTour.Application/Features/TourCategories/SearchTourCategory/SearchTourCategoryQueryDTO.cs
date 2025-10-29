using AppBookingTour.Application.Features.TourCategories.GetTourCategoryById;
using AppBookingTour.Application.Features.Tours.SearchTours;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.TourCategories.SearchTourCategory;

public class TourCategoryFilter
{
    public string? Name { get; set; }
    public int? ParentCategoryId { get; set; }
}

public class SearchTourCategoriesResponse : BaseResponse
{
    public List<TourCategoryDTO> Categories { get; set; } = [];
    public PaginationMeta Meta { get; set; } = null!;
}