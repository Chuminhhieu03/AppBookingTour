using AppBookingTour.Application.Features.TourTypes.GetTourTypeById;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourTypes.CreateTourType;

public sealed class CreateTourTypeCommandHandler : IRequestHandler<CreateTourTypeCommand, TourTypeDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<CreateTourTypeCommandHandler> _logger;
    private readonly IMapper _mapper;

    public CreateTourTypeCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ILogger<CreateTourTypeCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<TourTypeDTO> Handle(CreateTourTypeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating a new tour type");
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        var tourType = _mapper.Map<TourType>(request.RequestDto);
        var image = request.RequestDto.Image;

        if (image != null)
        {
            if (!allowedTypes.Contains(image.ContentType))
            {
                _logger.LogWarning("Invalid image type: {ImageType}", image.ContentType);
                throw new ArgumentException("Invalid image type. Allowed types are JPEG, PNG, WEBP.");
            }
            var fileUrl = await _fileStorageService.UploadFileAsync(image.OpenReadStream());
            tourType.ImageUrl = fileUrl;
        }

        tourType.CreatedAt = DateTime.UtcNow;
        tourType.IsActive = request.RequestDto.IsActive ?? true;

        await _unitOfWork.TourTypes.AddAsync(tourType, cancellationToken);
        var records = await _unitOfWork.SaveChangesAsync(cancellationToken);

        var tourTypeDto = _mapper.Map<TourTypeDTO>(tourType);

        _logger.LogInformation("Tour type created successfully with ID: {TourTypeId}", tourType.Id);
        return tourTypeDto;
    }
}