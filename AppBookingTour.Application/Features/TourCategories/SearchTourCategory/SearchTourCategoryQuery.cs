using MediatR;

namespace AppBookingTour.Application.Features.TourCategories.SearchTourCategory;

public record SearchTourCategoriesQuery(int? PageIndex, int? PageSize, TourCategoryFilter Filter) : IRequest<SearchTourCategoriesResponse>;
