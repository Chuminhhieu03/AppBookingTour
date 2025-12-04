using MediatR;
using Microsoft.Extensions.Logging;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.Combos.DeleteComboGalleryImages;

public sealed class DeleteComboGalleryImagesCommandHandler : IRequestHandler<DeleteComboGalleryImagesCommand, DeleteComboGalleryImagesResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<DeleteComboGalleryImagesCommandHandler> _logger;

    public DeleteComboGalleryImagesCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ILogger<DeleteComboGalleryImagesCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _logger = logger;
    }

    public async Task<DeleteComboGalleryImagesResponse> Handle(DeleteComboGalleryImagesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting gallery images for combo {ComboId}, Count: {Count}", request.ComboId, request.ImageUrls.Count);

        // Validate request
        if (request.ImageUrls == null || request.ImageUrls.Count == 0)
        {
            return DeleteComboGalleryImagesResponse.Failed("Không có ?nh nào ?? xóa");
        }

        // Ki?m tra combo t?n t?i s? d?ng ComboRepository
        var comboExists = await _unitOfWork.Combos.ExistsAsync(c => c.Id == request.ComboId, cancellationToken);
        if (!comboExists)
        {
            _logger.LogWarning("Combo with ID {ComboId} not found", request.ComboId);
            return DeleteComboGalleryImagesResponse.Failed($"Combo v?i ID {request.ComboId} không t?n t?i");
        }

        // L?y các ?nh t? database s? d?ng ImageRepository
        var images = await _unitOfWork.Images.GetListImageByEntityIdAndEntityType(request.ComboId, EntityType.Combo);
        
        int deletedCount = 0;

        foreach (var imageUrl in request.ImageUrls)
        {
            try
            {
                // Tìm image entity trong database
                var imageEntity = images.FirstOrDefault(img => img.Url == imageUrl);
                
                if (imageEntity != null)
                {
                    // Xóa file t? storage
                    try
                    {
                        await _fileStorageService.DeleteFileAsync(imageUrl);
                        _logger.LogInformation("Deleted gallery image file from storage: {Url}", imageUrl);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error deleting gallery image file from storage: {Url}", imageUrl);
                        // Continue to delete from database even if file deletion fails
                    }

                    // Xóa kh?i database s? d?ng ImageRepository
                    _unitOfWork.Images.Remove(imageEntity);
                    deletedCount++;
                }
                else
                {
                    _logger.LogWarning("Image not found in database: {Url}", imageUrl);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting gallery image: {Url}", imageUrl);
            }
        }

        if (deletedCount > 0)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        _logger.LogInformation("Successfully deleted {Count} gallery images for combo {ComboId}", deletedCount, request.ComboId);
        return DeleteComboGalleryImagesResponse.Success(deletedCount);
    }
}
