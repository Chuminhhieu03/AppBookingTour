using AppBookingTour.Application.IRepositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourCategories.GetTourCategoryById;

public sealed class GetTourCategoryByIdQueryHandler : IRequestHandler<GetTourCategoryByIdQuery, TourCategoryDTO>
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

    public async Task<TourCategoryDTO> Handle(GetTourCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting tour category details for ID: {TourCategoryId}", request.TourCategoryId);

        var tourCategory = await _unitOfWork.TourCategories
                .GetByIdAsync(request.TourCategoryId);

        if (tourCategory == null)
        {
            _logger.LogWarning("Tour category not found with ID: {TourCategoryId}", request.TourCategoryId);
            throw new KeyNotFoundException($"Tour category with ID {request.TourCategoryId} not found.");
        }

        var tourCategoryDto = _mapper.Map<TourCategoryDTO>(tourCategory);

        _logger.LogInformation("Successfully retrieved tour category details for ID: {TourCategoryId}", request.TourCategoryId);
        return tourCategoryDto;
    }
}