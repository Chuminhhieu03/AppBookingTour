using AppBookingTour.Application.Features.TourCategories.GetTourCategoryById;
using MediatR;

namespace AppBookingTour.Application.Features.TourCategories.GetTourCategoriesList;

public record GetTourCategoriesListQuery() : IRequest<List<TourCategoryDTO>>;