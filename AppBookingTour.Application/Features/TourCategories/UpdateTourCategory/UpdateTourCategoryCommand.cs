using AppBookingTour.Application.Features.TourCategories.CreateTourCategory;
using MediatR;

namespace AppBookingTour.Application.Features.TourCategories.UpdateTourCategory;

public record UpdateTourCategoryCommand(int TourCategoryId, TourCategoryRequestDTO RequestDto) : IRequest<UpdateTourCategoryResponse>;