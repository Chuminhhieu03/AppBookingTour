using MediatR;
using Microsoft.Extensions.Logging;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.Combos.UploadComboImages;

public sealed class UploadComboImagesCommandHandler : IRequestHandler<UploadComboImagesCommand, UploadComboImagesResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<UploadComboImagesCommandHandler> _logger;

    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private static readonly string[] AllowedContentTypes = ["image/jpeg", "image/png", "image/webp"];
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5MB

    public UploadComboImagesCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ILogger<UploadComboImagesCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _logger = logger;
    }

    public async Task<UploadComboImagesResponse> Handle(UploadComboImagesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Uploading images for combo {ComboId}", request.ComboId);

        // Kiểm tra combo tồn tại sử dụng ComboRepository
        var combo = await _unitOfWork.Combos.GetByIdAsync(request.ComboId, cancellationToken);
        if (combo == null)
        {
            return UploadComboImagesResponse.Failed($"Combo với ID {request.ComboId} không tồn tại");
        }

        string? coverImageUrl = null;
        var imageUrls = new List<string>();

        // Upload cover image
        if (request.CoverImage != null)
        {
            ValidateImageFile(request.CoverImage);
            
            using var stream = request.CoverImage.OpenReadStream();
            coverImageUrl = await _fileStorageService.UploadFileAsync(stream);
            
            // Cập nhật cover image sử dụng repository method
            await _unitOfWork.Combos.UpdateCoverImageAsync(request.ComboId, coverImageUrl, cancellationToken);
            _logger.LogInformation("Uploaded cover image for combo {ComboId}: {Url}", request.ComboId, coverImageUrl);
        }

        // Upload additional images
        if (request.Images != null && request.Images.Length > 0)
        {
            if (request.Images.Length > 10)
            {
                return UploadComboImagesResponse.Failed("Số lượng ảnh không được vượt quá 10");
            }

            foreach (var image in request.Images)
            {
                ValidateImageFile(image);
                
                using var stream = image.OpenReadStream();
                var imageUrl = await _fileStorageService.UploadFileAsync(stream);
                imageUrls.Add(imageUrl);

                // Save to Images table sử dụng ImageRepository
                var imageEntity = new Image
                {
                    Url = imageUrl,
                    EntityType = EntityType.Combo,
                    EntityId = request.ComboId,
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.Images.AddAsync(imageEntity, cancellationToken);
            }

            _logger.LogInformation("Uploaded {Count} images for combo {ComboId}", imageUrls.Count, request.ComboId);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully uploaded images for combo {ComboId}", request.ComboId);
        return UploadComboImagesResponse.Success(coverImageUrl, imageUrls);
    }

    private void ValidateImageFile(Microsoft.AspNetCore.Http.IFormFile file)
    {
        // Validate extension
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException(
                $"Định dạng file không hợp lệ. Chỉ chấp nhận: {string.Join(", ", AllowedExtensions)}");
        }

        // Validate content type
        if (!AllowedContentTypes.Contains(file.ContentType.ToLowerInvariant()))
        {
            throw new InvalidOperationException(
                $"Content type không hợp lệ. Chỉ chấp nhận: {string.Join(", ", AllowedContentTypes)}");
        }

        // Validate file size
        if (file.Length > MaxFileSizeBytes)
        {
            throw new InvalidOperationException("Kích thước file không được vượt quá 5MB");
        }

        // Validate file not empty
        if (file.Length == 0)
        {
            throw new InvalidOperationException("File không được rỗng");
        }
    }
}
