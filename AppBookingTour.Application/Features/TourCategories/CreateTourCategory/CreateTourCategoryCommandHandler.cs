using AppBookingTour.Application.Features.TourCategories.GetTourCategoryById;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourCategories.CreateTourCategory;

public sealed class CreateTourCategoryCommandHandler : IRequestHandler<CreateTourCategoryCommand, TourCategoryDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<CreateTourCategoryCommandHandler> _logger;
    private readonly IMapper _mapper;

    public CreateTourCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ILogger<CreateTourCategoryCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<TourCategoryDTO> Handle(CreateTourCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating a new tour category");
        
        var existingTourCategoryByName = await _unitOfWork.TourCategories.FirstOrDefaultAsync(x => x.Name == request.RequestDto.Name);
        if (existingTourCategoryByName != null)
        {
            throw new ArgumentException(string.Format(Message.AlreadyExists, "Tên danh mục tour"));
        }

        if (request.RequestDto.ParentCategoryId.HasValue)
        {
            var parentExists = await _unitOfWork.TourCategories
                .ExistsAsync(c => c.Id == request.RequestDto.ParentCategoryId.Value, cancellationToken);

            if (!parentExists)
            {
                _logger.LogWarning("Invalid ParentCategoryId: {ParentId}", request.RequestDto.ParentCategoryId.Value);
                throw new KeyNotFoundException($"Parent category with ID {request.RequestDto.ParentCategoryId.Value} not found");
            }
        }

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        var image = request.RequestDto.Image;

        var tourCategory = _mapper.Map<TourCategory>(request.RequestDto);

        if (image != null)
        {
            if (!allowedTypes.Contains(image.ContentType))
            {
                _logger.LogWarning("Invalid image type: {ImageType}", image.ContentType);
                throw new ArgumentException(Message.InvalidImage);
            }
            var fileUrl = await _fileStorageService.UploadFileAsync(image.OpenReadStream());
            tourCategory.ImageUrl = fileUrl;
        }

        tourCategory.CreatedAt = DateTime.UtcNow;
        tourCategory.IsActive = request.RequestDto.IsActive ?? true;

        await _unitOfWork.TourCategories.AddAsync(tourCategory, cancellationToken);
        int records = await _unitOfWork.SaveChangesAsync(cancellationToken);

        var categoryDto = _mapper.Map<TourCategoryDTO>(tourCategory);

        _logger.LogInformation("Tour category created successfully with ID: {TourCategoryId}", tourCategory.Id);
        return categoryDto;
    }
}