using MediatR;

namespace AppBookingTour.Application.Features.TourCategories.GetTourCategoryById;

public record GetTourCategoryByIdQuery(int TourCategoryId) : IRequest<TourCategoryDTO>;