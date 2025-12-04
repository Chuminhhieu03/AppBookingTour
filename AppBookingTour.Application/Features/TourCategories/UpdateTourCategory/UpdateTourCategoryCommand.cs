using AppBookingTour.Application.Features.TourCategories.CreateTourCategory;
using AppBookingTour.Application.Features.TourCategories.GetTourCategoryById;
using MediatR;

namespace AppBookingTour.Application.Features.TourCategories.UpdateTourCategory;

public record UpdateTourCategoryCommand(int TourCategoryId, TourCategoryRequestDTO RequestDto) : IRequest<TourCategoryDTO>;