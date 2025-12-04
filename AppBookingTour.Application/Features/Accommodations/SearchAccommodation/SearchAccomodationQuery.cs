using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.SearchAccommodation
{
    public record class SearchAccomodationQuery(int? pageIndex, int? pageSize, SearchAccommodationFilter? SearchAccommodationFilter) : IRequest<SearchAccommodationResponse>;
}
