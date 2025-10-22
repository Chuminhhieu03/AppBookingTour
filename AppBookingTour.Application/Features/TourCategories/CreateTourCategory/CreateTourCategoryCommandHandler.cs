using AppBookingTour.Application.Features.TourCategories.GetTourCategoryById;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourCategories.CreateTourCategory;

public sealed class CreateTourCategoryCommandHandler : IRequestHandler<CreateTourCategoryCommand, CreateTourCategoryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateTourCategoryCommandHandler> _logger;
    private readonly IMapper _mapper;

    public CreateTourCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateTourCategoryCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CreateTourCategoryResponse> Handle(CreateTourCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating a new tour category");
        try
        {
            if (request.RequestDto.ParentCategoryId.HasValue)
            {
                var parentExists = await _unitOfWork.Repository<TourCategory>()
                    .ExistsAsync(c => c.Id == request.RequestDto.ParentCategoryId.Value, cancellationToken);

                if (!parentExists)
                {
                    _logger.LogWarning("Invalid ParentCategoryId: {ParentId}", request.RequestDto.ParentCategoryId.Value);
                    return CreateTourCategoryResponse.Failed($"Parent category with ID {request.RequestDto.ParentCategoryId.Value} not found.");
                }
            }

            var tourCategory = _mapper.Map<TourCategory>(request.RequestDto);

            tourCategory.CreatedAt = DateTime.UtcNow;
            tourCategory.IsActive = request.RequestDto.IsActive ?? true;

            await _unitOfWork.Repository<TourCategory>().AddAsync(tourCategory, cancellationToken);
            int records = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (records == 0)
            {
                _logger.LogWarning("Create tour category failed, no records affected");
                return CreateTourCategoryResponse.Failed("Create tour category failed, no records affected");
            }

            var categoryDto = _mapper.Map<TourCategoryDTO>(tourCategory);

            _logger.LogInformation("Tour category created successfully with ID: {TourCategoryId}", tourCategory.Id);
            return CreateTourCategoryResponse.Success(categoryDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour category");
            return CreateTourCategoryResponse.Failed("Create tour category failed: " + ex.Message);
        }
    }
}