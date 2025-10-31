using AppBookingTour.Application.Features.Tours.SearchTours;
using AppBookingTour.Application.Features.TourTypes.GetTourTypeById; 
using AppBookingTour.Application.IRepositories;

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourTypes.SearchTourType;

public class SearchTourTypesQueryHandler : IRequestHandler<SearchTourTypesQuery, SearchTourTypesResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SearchTourTypesQueryHandler> _logger;
    private readonly IMapper _mapper;

    public SearchTourTypesQueryHandler(IUnitOfWork unitOfWork, ILogger<SearchTourTypesQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<SearchTourTypesResponse> Handle(SearchTourTypesQuery request, CancellationToken cancellationToken)
    {
        int pageIndex = request.PageIndex ?? 1;
        int pageSize = request.PageSize ?? 10;

        _logger.LogInformation("Searching Tour Types with filter: {@Filter} for Page: {Page}, PageSize: {PageSize}",
            request.Filter, pageIndex, pageSize);

        var (types, totalCount) = await _unitOfWork.TourTypes.SearchTourTypesAsync(request.Filter, pageIndex, pageSize, cancellationToken);

        var typeListItems = _mapper.Map<List<TourTypeDTO>>(types);

        var totalPages = (pageSize == 0) ? 0 : (int)Math.Ceiling((double)totalCount / pageSize);

        return new SearchTourTypesResponse
        {
            Success = true,
            TourTypes = typeListItems,
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