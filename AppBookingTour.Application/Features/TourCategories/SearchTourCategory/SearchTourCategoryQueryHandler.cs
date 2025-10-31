using AppBookingTour.Application.Features.TourCategories.GetTourCategoryById;
using AppBookingTour.Application.Features.Tours.SearchTours;
using AppBookingTour.Application.IRepositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourCategories.SearchTourCategory;

public class SearchTourCategoriesQueryHandler : IRequestHandler<SearchTourCategoriesQuery, SearchTourCategoriesResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SearchTourCategoriesQueryHandler> _logger;
    private readonly IMapper _mapper;

    public SearchTourCategoriesQueryHandler(IUnitOfWork unitOfWork, ILogger<SearchTourCategoriesQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<SearchTourCategoriesResponse> Handle(SearchTourCategoriesQuery request, CancellationToken cancellationToken)
    {
        int pageIndex = request.PageIndex ?? 1;
        int pageSize = request.PageSize ?? 10;

        _logger.LogInformation("Searching Tour Categories with filter: {@Filter} for Page: {Page}, PageSize: {PageSize}",
            request.Filter, pageIndex, pageSize);

        var (categories, totalCount) = await _unitOfWork.TourCategories.SearchTourCategoriesAsync(request.Filter, pageIndex, pageSize, cancellationToken);

        var categoryListItems = _mapper.Map<List<TourCategoryDTO>>(categories);

        var totalPages = (pageSize == 0) ? 0 : (int)Math.Ceiling((double)totalCount / pageSize);

        return new SearchTourCategoriesResponse
        {
            Success = true,
            Categories = categoryListItems,
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
