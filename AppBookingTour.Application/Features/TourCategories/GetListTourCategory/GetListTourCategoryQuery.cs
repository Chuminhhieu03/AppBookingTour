using MediatR;

namespace AppBookingTour.Application.Features.TourCategories.GetTourCategoriesList;

public record GetTourCategoriesListQuery() : IRequest<GetTourCategoriesListResponse>;