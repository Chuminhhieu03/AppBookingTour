using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Constants;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Profiles.UpdateProfile;

public sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<UpdateProfileCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateProfileCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ILogger<UpdateProfileCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating user profile for ID: {UserId}", request.Id);

        var existingProfile = await _unitOfWork.Profiles.GetUserByIdAsync(request.Id, cancellationToken);

        if (existingProfile == null)
        {
            _logger.LogWarning("Profile not found for update with ID: {UserId}", request.Id);
            throw new KeyNotFoundException($"Profile with ID {request.Id} not found.");
        }

        _mapper.Map(request.Dto, existingProfile);

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        var imageFile = request.Dto.ProfileImageFile;
        if (imageFile != null)
        {
            if (!allowedTypes.Contains(imageFile.ContentType))
            {
                throw new ArgumentException(Message.InvalidImage);
            }
            var oldImageUrl = existingProfile.ProfileImage;
            if (!string.IsNullOrEmpty(oldImageUrl))
            {
                await _fileStorageService.DeleteFileAsync(oldImageUrl);
            }
            var fileUrl = await _fileStorageService.UploadFileAsync(imageFile.OpenReadStream());
            existingProfile.ProfileImage = fileUrl;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Update user profile successfully with userId : {UserId}", request.Id);
        return new Unit();
    }
}