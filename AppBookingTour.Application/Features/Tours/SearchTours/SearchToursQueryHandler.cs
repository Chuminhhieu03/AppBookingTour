using AppBookingTour.Application.IRepositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Tours.SearchTours
{
    public class SearchToursQueryHandler : IRequestHandler<SearchToursQuery, SearchToursResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SearchToursQueryHandler> _logger;
        private readonly IMapper _mapper;

        public SearchToursQueryHandler(IUnitOfWork unitOfWork, ILogger<SearchToursQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<SearchToursResponse> Handle(SearchToursQuery request, CancellationToken cancellationToken)
        {
            try
            {
                int pageIndex = request.PageIndex.HasValue ? request.PageIndex.Value : 1;
                int pageSize = request.PageSize.HasValue ? request.PageSize.Value : 10;

                _logger.LogInformation("Searching tours with filter: {@Filter} for Page: {Page}, PageSize: {PageSize}",
                    request.Filter, pageIndex, pageSize);

                var (tours, totalCount) = await _unitOfWork.Tours.SearchToursAsync(
                    request.Filter,
                    pageIndex,
                    pageSize,
                    cancellationToken);

                var tourListItems = _mapper.Map<List<TourListItem>>(tours);

                var totalPages = (request.PageSize == 0) ? 0 : (int)Math.Ceiling((double)totalCount / pageSize);

                return new SearchToursResponse
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching tours in handler for filter: {@Filter}", request.Filter);
                throw;
            }
        }
    }
}