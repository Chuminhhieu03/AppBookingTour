using AppBookingTour.Application.IRepositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using AppBookingTour.Application.Features.Tours.SearchTours;

namespace AppBookingTour.Application.Features.Tours.SearchToursForCustomer
{
    public class SearchToursForCustomerQueryHandler : IRequestHandler<SearchToursForCustomerQuery, SearchToursForCustomerResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SearchToursForCustomerQueryHandler> _logger;
        private readonly IMapper _mapper;

        public SearchToursForCustomerQueryHandler(IUnitOfWork unitOfWork, ILogger<SearchToursForCustomerQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<SearchToursForCustomerResponse> Handle(SearchToursForCustomerQuery request, CancellationToken cancellationToken)
        {
            int pageIndex = request.PageIndex.HasValue ? request.PageIndex.Value : 1;
            int pageSize = request.PageSize.HasValue ? request.PageSize.Value : 10;

            _logger.LogInformation("Searching tours for CUSTOMER with filter: {@Filter} for Page: {Page}, PageSize: {PageSize}",
                request.Filter, pageIndex, pageSize);

            var (tours, totalCount) = await _unitOfWork.Tours.SearchToursForCustomerAsync(
                request.Filter,
                pageIndex,
                pageSize,
                cancellationToken);

            var tourListItems = _mapper.Map<List<CustomerTourListItem>>(tours);

            if (request.Filter.DepartureDate.HasValue)
            {
                var filterDate = request.Filter.DepartureDate.Value;
                foreach (var tourItem in tourListItems)
                {
                    tourItem.Departures = tourItem.Departures
                        .Where(d => DateOnly.FromDateTime(d.DepartureDate) >= filterDate)
                        .OrderBy(d => d.DepartureDate)
                        .ToList();
                }
            }

            var totalPages = (pageSize == 0) ? 0 : (int)Math.Ceiling((double)totalCount / pageSize);

            return new SearchToursForCustomerResponse
            {
                Tours = tourListItems,
                Meta = new PaginationMeta
                {
                    TotalCount = totalCount,
                    Page = pageIndex,
                    PageSize = pageSize,
                    TotalPages = totalPages
                }
            };
        }
    }
}