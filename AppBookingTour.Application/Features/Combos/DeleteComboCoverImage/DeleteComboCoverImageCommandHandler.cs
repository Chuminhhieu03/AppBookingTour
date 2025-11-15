using MediatR;
using Microsoft.Extensions.Logging;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;

namespace AppBookingTour.Application.Features.Combos.DeleteComboCoverImage;

public sealed class DeleteComboCoverImageCommandHandler : IRequestHandler<DeleteComboCoverImageCommand, DeleteComboCoverImageResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<DeleteComboCoverImageCommandHandler> _logger;

    public DeleteComboCoverImageCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ILogger<DeleteComboCoverImageCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _logger = logger;
    }

    public async Task<DeleteComboCoverImageResponse> Handle(DeleteComboCoverImageCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting cover image for combo {ComboId}", request.ComboId);

        // Kiểm tra combo tồn tại
        var combo = await _unitOfWork.Combos.GetByIdAsync(request.ComboId, cancellationToken);
        if (combo == null)
        {
            _logger.LogWarning("Combo with ID {ComboId} not found", request.ComboId);
            return DeleteComboCoverImageResponse.Failed($"Combo với ID {request.ComboId} không tồn tại");
        }

        // Kiểm tra có ảnh bìa không
        if (string.IsNullOrEmpty(combo.ComboImageCoverUrl))
        {
            _logger.LogInformation("Combo {ComboId} has no cover image to delete", request.ComboId);
            return DeleteComboCoverImageResponse.Success();
        }

        var coverImageUrl = combo.ComboImageCoverUrl;
        
        await _fileStorageService.DeleteFileAsync(coverImageUrl);
        _logger.LogInformation("Deleted cover image file from storage: {Url}", coverImageUrl);

        // Cập nhật database sử dụng repository method
        await _unitOfWork.Combos.UpdateCoverImageAsync(request.ComboId, null, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully deleted cover image for combo {ComboId}", request.ComboId);
        return DeleteComboCoverImageResponse.Success();
    }
}
