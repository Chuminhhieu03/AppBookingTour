using AppBookingTour.Application.Features.TourCategories.GetTourCategoryById;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourCategories.UpdateTourCategory;

public sealed class UpdateTourCategoryCommandHandler : IRequestHandler<UpdateTourCategoryCommand, TourCategoryDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<UpdateTourCategoryCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateTourCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ILogger<UpdateTourCategoryCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<TourCategoryDTO> Handle(UpdateTourCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Tour category updating with ID: {TourCategoryId}", request.TourCategoryId);
        var existingCategory = await _unitOfWork.TourCategories.GetByIdAsync(request.TourCategoryId, cancellationToken);
        if (existingCategory == null)
        {
            _logger.LogWarning("Tour category with ID {TourCategoryId} not found.", request.TourCategoryId);
            throw new KeyNotFoundException($"Tour category with ID {request.TourCategoryId} not found.");
        }

        if (request.RequestDto.ParentCategoryId.HasValue)
        {
            if (request.RequestDto.ParentCategoryId == request.TourCategoryId)
            {
                throw new ArgumentException("A category cannot be its own parent category.");
            }

            var parentExists = await _unitOfWork.TourCategories
                .ExistsAsync(c => c.Id == request.RequestDto.ParentCategoryId.Value, cancellationToken);

            if (!parentExists)
            {
                _logger.LogWarning("Invalid ParentCategoryId: {ParentId}", request.RequestDto.ParentCategoryId.Value);
                throw new KeyNotFoundException($"Parent category with ID {request.RequestDto.ParentCategoryId.Value} not found.");
            }
        }

        _mapper.Map(request.RequestDto, existingCategory);

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        var imageFile = request.RequestDto.Image;
        if (imageFile != null)
        {
            if (!allowedTypes.Contains(imageFile.ContentType))
            {
                _logger.LogWarning("Invalid image type for tour category ID {TourCategoryId}.", request.TourCategoryId);
                throw new ArgumentException("Invalid image type.");
            }
            var oldImageUrl = existingCategory.ImageUrl;
            if (!string.IsNullOrEmpty(oldImageUrl))
            {
                await _fileStorageService.DeleteFileAsync(oldImageUrl);
            }
            var fileUrl = await _fileStorageService.UploadFileAsync(imageFile.OpenReadStream());
            existingCategory.ImageUrl = fileUrl;
        }

        existingCategory.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Tour category updated with ID: {TourCategoryId}", request.TourCategoryId);
        var tourCategoryDto = _mapper.Map<TourCategoryDTO>(existingCategory);

        return tourCategoryDto;
    }
}