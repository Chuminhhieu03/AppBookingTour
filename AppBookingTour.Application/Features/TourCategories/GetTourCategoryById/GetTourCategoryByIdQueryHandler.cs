using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourCategories.GetTourCategoryById;

public sealed class GetTourCategoryByIdQueryHandler : IRequestHandler<GetTourCategoryByIdQuery, GetTourCategoryByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetTourCategoryByIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetTourCategoryByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetTourCategoryByIdQueryHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<GetTourCategoryByIdResponse> Handle(GetTourCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting tour category details for ID: {TourCategoryId}", request.TourCategoryId);

        try
        {
            var tourCategory = await _unitOfWork.Repository<TourCategory>()
                .GetByIdAsync(request.TourCategoryId);

            if (tourCategory == null)
            {
                _logger.LogWarning("Tour category not found with ID: {TourCategoryId}", request.TourCategoryId);
                return GetTourCategoryByIdResponse.Failed($"Tour category with ID {request.TourCategoryId} not found");
            }

            var tourCategoryDto = _mapper.Map<TourCategoryDTO>(tourCategory);

            _logger.LogInformation("Successfully retrieved tour category details for ID: {TourCategoryId}", request.TourCategoryId);
            return GetTourCategoryByIdResponse.Success(tourCategoryDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting tour category details for ID: {TourCategoryId}", request.TourCategoryId);
            return GetTourCategoryByIdResponse.Failed("An error occurred while retrieving tour category details");
        }
    }
}