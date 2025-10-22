using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourCategories.UpdateTourCategory;

public sealed class UpdateTourCategoryCommandHandler : IRequestHandler<UpdateTourCategoryCommand, UpdateTourCategoryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateTourCategoryCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateTourCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateTourCategoryCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<UpdateTourCategoryResponse> Handle(UpdateTourCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Tour category updating with ID: {TourCategoryId}", request.TourCategoryId);
        try
        {
            var existingCategory = await _unitOfWork.Repository<TourCategory>().GetByIdAsync(request.TourCategoryId, cancellationToken);
            if (existingCategory == null)
            {
                _logger.LogWarning("Tour category with ID {TourCategoryId} not found.", request.TourCategoryId);
                return UpdateTourCategoryResponse.Failed($"Tour category with ID {request.TourCategoryId} not found.");
            }

            if (request.RequestDto.ParentCategoryId.HasValue)
            {
                if (request.RequestDto.ParentCategoryId == request.TourCategoryId)
                {
                    return UpdateTourCategoryResponse.Failed("Category cannot be its own parent.");
                }

                var parentExists = await _unitOfWork.Repository<TourCategory>()
                    .ExistsAsync(c => c.Id == request.RequestDto.ParentCategoryId.Value, cancellationToken);

                if (!parentExists)
                {
                    _logger.LogWarning("Invalid ParentCategoryId: {ParentId}", request.RequestDto.ParentCategoryId.Value);
                    return UpdateTourCategoryResponse.Failed($"Parent category with ID {request.RequestDto.ParentCategoryId.Value} not found.");
                }
            }

            _mapper.Map(request.RequestDto, existingCategory);

            existingCategory.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Tour category updated with ID: {TourCategoryId}", request.TourCategoryId);

            return UpdateTourCategoryResponse.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating tour category.");
            return UpdateTourCategoryResponse.Failed("An error occurred while updating the tour category.");
        }
    }
}