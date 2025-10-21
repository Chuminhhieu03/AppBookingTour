using AppBookingTour.Application.Features.TourTypes.CreateTourType;
using MediatR;

namespace AppBookingTour.Application.Features.TourTypes.UpdateTourType;

public record UpdateTourTypeCommand(int TourTypeId, TourTypeRequestDTO RequestDto) : IRequest<UpdateTourTypeResponse>;