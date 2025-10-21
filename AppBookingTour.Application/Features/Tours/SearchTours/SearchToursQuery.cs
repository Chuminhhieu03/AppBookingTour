using MediatR;

namespace AppBookingTour.Application.Features.Tours.SearchTours;

public record SearchToursQuery(int? PageIndex, int? PageSize, SearchTourFilter Filter) : IRequest<SearchToursResponse>;