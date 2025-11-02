using MediatR;

namespace AppBookingTour.Application.Features.TourTypes.GetTourTypeById;

public record GetTourTypeByIdQuery(int TourTypeId) : IRequest<TourTypeDTO>;