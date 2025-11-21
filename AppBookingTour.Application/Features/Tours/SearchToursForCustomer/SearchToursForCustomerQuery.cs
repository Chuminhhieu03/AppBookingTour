using MediatR;

namespace AppBookingTour.Application.Features.Tours.SearchToursForCustomer;

public record SearchToursForCustomerQuery(int? PageIndex, int? PageSize, SearchToursForCustomerFilter Filter) : IRequest<SearchToursForCustomerResponse>;