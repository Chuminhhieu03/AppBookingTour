using MediatR;

namespace AppBookingTour.Application.Features.TourTypes.DeleteTourType;

public record DeleteTourTypeCommand(int TourTypeId) : IRequest<DeleteTourTypeResponse>;