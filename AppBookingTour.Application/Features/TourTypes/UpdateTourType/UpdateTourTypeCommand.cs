using AppBookingTour.Application.Features.TourTypes.CreateTourType;
using AppBookingTour.Application.Features.TourTypes.GetTourTypeById;
using MediatR;

namespace AppBookingTour.Application.Features.TourTypes.UpdateTourType;

public record UpdateTourTypeCommand(int TourTypeId, TourTypeRequestDTO RequestDto) : IRequest<TourTypeDTO>;