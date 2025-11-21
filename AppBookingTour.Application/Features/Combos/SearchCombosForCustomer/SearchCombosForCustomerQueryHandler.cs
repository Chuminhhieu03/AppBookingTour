using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.Features.Tours.SearchTours;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Combos.SearchCombosForCustomer
{
    public class SearchCombosForCustomerQueryHandler : IRequestHandler<SearchCombosForCustomerQuery, SearchCombosForCustomerResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SearchCombosForCustomerQueryHandler> _logger;
        private readonly IMapper _mapper;

        public SearchCombosForCustomerQueryHandler(IUnitOfWork unitOfWork, ILogger<SearchCombosForCustomerQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<SearchCombosForCustomerResponse> Handle(SearchCombosForCustomerQuery request, CancellationToken cancellationToken)
        {
            int pageIndex = request.PageIndex ?? 1;
            int pageSize = request.PageSize ?? 10;

            _logger.LogInformation("Searching combos for CUSTOMER with filter: {@Filter} for Page: {Page}, PageSize: {PageSize}",
                request.Filter, pageIndex, pageSize);

            var (combos, totalCount) = await _unitOfWork.Combos.SearchCombosForCustomerAsync(
                request.Filter,
                pageIndex,
                pageSize,
                cancellationToken);

            var comboListItems = _mapper.Map<List<CustomerComboListItem>>(combos);


            if (request.Filter.DepartureDate.HasValue)
            {
                var filterDate = request.Filter.DepartureDate.Value;
                foreach (var comboItem in comboListItems)
                {
                    comboItem.Schedules = comboItem.Schedules
                        .Where(s => DateOnly.FromDateTime(s.DepartureDate) >= filterDate)
                        .OrderBy(s => s.DepartureDate)
                        .ToList();
                }
            }

            var totalPages = (pageSize == 0) ? 0 : (int)Math.Ceiling((double)totalCount / pageSize);

            return new SearchCombosForCustomerResponse
            {
                Combos = comboListItems,
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