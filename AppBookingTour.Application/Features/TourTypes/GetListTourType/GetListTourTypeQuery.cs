using MediatR;

namespace AppBookingTour.Application.Features.TourTypes.GetTourTypesList;

public record GetTourTypesListQuery() : IRequest<GetTourTypesListResponse>;