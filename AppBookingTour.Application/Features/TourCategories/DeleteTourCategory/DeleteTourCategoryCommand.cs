using MediatR;

namespace AppBookingTour.Application.Features.TourCategories.DeleteTourCategory;

public record DeleteTourCategoryCommand(int TourCategoryId) : IRequest<DeleteTourCategoryResponse>;