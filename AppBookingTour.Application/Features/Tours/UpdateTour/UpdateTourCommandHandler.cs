using AppBookingTour.Application.Features.Tours.GetTourById;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Constants;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Tours.UpdateTour;

#region Handler
public sealed class UpdateTourComandHandler : IRequestHandler<UpdateTourCommand, TourDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<UpdateTourComandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateTourComandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ILogger<UpdateTourComandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<TourDTO> Handle(UpdateTourCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Tour updating with ID: {TourId}", request.TourId);
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };

        var existingTour = await _unitOfWork.Tours.GetByIdAsync(request.TourId, cancellationToken);
        if (existingTour == null)
        {
            throw new KeyNotFoundException($"Tour with ID {request.TourId} not found.");
        }

        var existingTourByCode = await _unitOfWork.Tours.FirstOrDefaultAsync(x => x.Code == request.TourRequest.Code);
        if (existingTourByCode != null && existingTourByCode.Id != existingTour.Id)
        {
            throw new ArgumentException(string.Format(Message.AlreadyExists, "Mã tour"));
        }

        _mapper.Map(request.TourRequest, existingTour);

        var imageMain = request.TourRequest.ImageMain;
        var images = request.TourRequest.Images;
        var removeImageUrls = request.TourRequest.RemoveImageUrls;

        string? newMainUrl = null;
        var newImageList = new List<Image>();

        if (imageMain != null)
        {
            if (!allowedTypes.Contains(imageMain.ContentType))
            {
                throw new ArgumentException(Message.InvalidImage);
            }

            newMainUrl = await _fileStorageService.UploadFileAsync(imageMain.OpenReadStream());
        }

        if (images != null && images.Count > 0)
        {
            foreach (var image in images)
            {
                if (!allowedTypes.Contains(image.ContentType))
                {
                    throw new ArgumentException(Message.InvalidImage);
                }
            }

            foreach (var image in images)
            {
                var fileUrl = await _fileStorageService.UploadFileAsync(image.OpenReadStream());
                newImageList.Add(new Image
                {
                    EntityId = existingTour.Id,
                    EntityType = Domain.Enums.EntityType.Tour,
                    Url = fileUrl
                });
            }
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var oldImageMainUrl = existingTour.ImageMainUrl;
            if (newMainUrl != null)
            {
                existingTour.ImageMainUrl = newMainUrl;
            }

            if (removeImageUrls != null && removeImageUrls.Count > 0)
            {
                await _unitOfWork.Images.RemoveRangeByImgUrls(removeImageUrls);
            }

            if (newImageList.Count > 0)
            {
                await _unitOfWork.Images.AddRangeAsync(newImageList, cancellationToken);
            }

            existingTour.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            // Xóa file hình ảnh khỏi lưu trữ
            if (oldImageMainUrl != existingTour.ImageMainUrl)
            {
                await _fileStorageService.DeleteFileAsync(oldImageMainUrl);
            }
            if (removeImageUrls != null && removeImageUrls.Count > 0)
            {
                foreach (var url in removeImageUrls)
                {
                    await _fileStorageService.DeleteFileAsync(url);
                }
            }

        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }

        _logger.LogInformation("Tour updated with ID: {TourId}", request.TourId);
        return _mapper.Map<TourDTO>(existingTour);
    }
};
#endregion
