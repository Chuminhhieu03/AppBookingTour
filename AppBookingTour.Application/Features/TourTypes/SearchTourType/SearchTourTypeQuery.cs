using MediatR;

namespace AppBookingTour.Application.Features.TourTypes.SearchTourType;

public record SearchTourTypesQuery(int? PageIndex, int? PageSize, TourTypeFilter Filter) : IRequest<SearchTourTypesResponse>;
