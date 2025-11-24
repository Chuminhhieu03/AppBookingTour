using MediatR;

namespace AppBookingTour.Application.Features.Combos.SearchCombosForCustomer;

public record SearchCombosForCustomerQuery(int? PageIndex, int? PageSize, SearchCombosForCustomerFilter Filter) : IRequest<SearchCombosForCustomerResponse>;