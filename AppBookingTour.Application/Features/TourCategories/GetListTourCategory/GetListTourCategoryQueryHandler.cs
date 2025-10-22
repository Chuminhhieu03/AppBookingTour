using AppBookingTour.Application.Features.TourCategories.GetTourCategoryById;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourCategories.GetTourCategoriesList;

public sealed class GetTourCategoriesListQueryHandler : IRequestHandler<GetTourCategoriesListQuery, GetTourCategoriesListResponse>
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

    public async Task<GetTourCategoriesListResponse> Handle(GetTourCategoriesListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all tour categories");
        try
        {
            var tourCategories = await _unitOfWork.Repository<TourCategory>()
                                       .FindAsync(predicate: c => true,
                                                  includes: c => c.ParentCategory);

            var tourCategoryDtos = _mapper.Map<List<TourCategoryDTO>>(tourCategories);

            tourCategoryDtos = tourCategoryDtos.OrderBy(c => c.Name).ToList();

            return GetTourCategoriesListResponse.Success(tourCategoryDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all tour categories");
            return GetTourCategoriesListResponse.Failed("An error occurred while retrieving tour categories.");
        }
    }
}