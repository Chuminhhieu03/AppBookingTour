using MediatR;

namespace AppBookingTour.Application.Features.TourCategories.CreateTourCategory;

public record CreateTourCategoryCommand(TourCategoryRequestDTO RequestDto) : IRequest<CreateTourCategoryResponse>;