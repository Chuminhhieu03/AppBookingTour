using AppBookingTour.Application.IRepositories;

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using AppBookingTour.Application.Features.Tours.SearchTours;


namespace AppBookingTour.Application.Features.Accommodations.SearchAccommodationsForCustomer
{
    public class SearchAccommodationsForCustomerQueryHandler : IRequestHandler<SearchAccommodationsForCustomerQuery, SearchAccommodationsForCustomerResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SearchAccommodationsForCustomerQueryHandler> _logger;

        public SearchAccommodationsForCustomerQueryHandler(IUnitOfWork unitOfWork, ILogger<SearchAccommodationsForCustomerQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<SearchAccommodationsForCustomerResponse> Handle(SearchAccommodationsForCustomerQuery request, CancellationToken cancellationToken)
        {
            int pageIndex = request.PageIndex ?? 1;
            int pageSize = request.PageSize ?? 10;

            _logger.LogInformation(
                "Calling SQL Repository to search accommodations. Filter: {@Filter}, Page: {Page}, PageSize: {PageSize}",
                request.Filter, pageIndex, pageSize);

            var (accommodations, totalCount) = await _unitOfWork.Accommodations.SearchAccommodationsForCustomerAsync(
                request.Filter,
                pageIndex,
                pageSize,
                cancellationToken);

            _logger.LogInformation("SQL query returned {Count} items with a TotalCount of {TotalCount}.", accommodations.Count, totalCount);

            var totalPages = (pageSize == 0) ? 0 : (int)Math.Ceiling((double)totalCount / pageSize);

            return new SearchAccommodationsForCustomerResponse
            {
                Accommodations = accommodations,
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