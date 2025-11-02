using AppBookingTour.Application.Features.TourTypes.GetTourTypeById;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Constants;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourTypes.UpdateTourType;

public sealed class UpdateTourTypeCommandHandler : IRequestHandler<UpdateTourTypeCommand, TourTypeDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<UpdateTourTypeCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateTourTypeCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ILogger<UpdateTourTypeCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<TourTypeDTO> Handle(UpdateTourTypeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Tour type updating with ID: {TourTypeId}", request.TourTypeId);
        var existingTourType = await _unitOfWork.TourTypes.GetByIdAsync(request.TourTypeId, cancellationToken);
        if (existingTourType == null)
        {
            _logger.LogWarning("Tour type with ID {TourTypeId} not found.", request.TourTypeId);
            throw new KeyNotFoundException($"Tour type with ID {request.TourTypeId} not found.");
        }

        var existingTourTypeByName = await _unitOfWork.TourTypes.FirstOrDefaultAsync(x => x.Name == request.RequestDto.Name);
        if (existingTourTypeByName != null && existingTourTypeByName.Id != existingTourType.Id)
        {
            throw new ArgumentException(string.Format(Message.AlreadyExists, "Tên loại tour"));
        }

        _mapper.Map(request.RequestDto, existingTourType);
        
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        var imageFile = request.RequestDto.Image;
        if (imageFile != null)
        {
            if (!allowedTypes.Contains(imageFile.ContentType))
            {
                _logger.LogWarning("Invalid image type for tour type ID {TourTypeId}.", request.TourTypeId);
                throw new ArgumentException(Message.InvalidImage);
            }
            var oldImageUrl = existingTourType.ImageUrl;
            if(!string.IsNullOrEmpty(oldImageUrl))
            {
                await _fileStorageService.DeleteFileAsync(oldImageUrl);
            }
            var fileUrl = await _fileStorageService.UploadFileAsync(imageFile.OpenReadStream());
            existingTourType.ImageUrl = fileUrl;
        }

        existingTourType.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var tourTypeDto = _mapper.Map<TourTypeDTO>(existingTourType);

        _logger.LogInformation("Tour type updated with ID: {TourTypeId}", request.TourTypeId);
        return tourTypeDto;
    }
}