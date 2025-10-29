using AppBookingTour.Application.Features.TourCategories.GetTourCategoryById;
using AppBookingTour.Application.IRepositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourCategories.GetTourCategoriesList;

public sealed class GetTourCategoriesListQueryHandler : IRequestHandler<GetTourCategoriesListQuery, List<TourCategoryDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetTourCategoriesListQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetTourCategoriesListQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetTourCategoriesListQueryHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<TourCategoryDTO>> Handle(GetTourCategoriesListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all tour categories");
        var tourCategories = await _unitOfWork.TourCategories
                                       .FindAsync(predicate: c => true,
                                                  includes: c => c.ParentCategory);

        var tourCategoryItems = _mapper.Map<List<TourCategoryDTO>>(tourCategories);

        tourCategoryItems = tourCategoryItems.OrderBy(c => c.Name).ToList();

        return tourCategoryItems;
    }
}