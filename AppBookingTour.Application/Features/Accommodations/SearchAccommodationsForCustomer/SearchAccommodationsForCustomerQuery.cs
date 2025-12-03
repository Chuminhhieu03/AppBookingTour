using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.SearchAccommodationsForCustomer;

public record SearchAccommodationsForCustomerQuery(int? PageIndex, int? PageSize, SearchAccommodationsForCustomerFilter Filter) : IRequest<SearchAccommodationsForCustomerResponse>;
