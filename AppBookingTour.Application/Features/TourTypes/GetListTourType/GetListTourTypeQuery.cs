using AppBookingTour.Application.Features.TourTypes.GetTourTypeById;
using MediatR;

namespace AppBookingTour.Application.Features.TourTypes.GetTourTypesList;

public record GetTourTypesListQuery() : IRequest<List<TourTypeDTO>>;