using AppBookingTour.Application.Features.Tours.GetTourById;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Tours.CreateTour;

#region Handler
public sealed class CreateTourCommandHandler : IRequestHandler<CreateTourCommand, TourDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<CreateTourCommandHandler> _logger;
    private readonly IMapper _mapper;

    public CreateTourCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ILogger<CreateTourCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<TourDTO> Handle(CreateTourCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating a new tour using AutoMapper");

        var existingTourByCode = await _unitOfWork.Tours.FirstOrDefaultAsync(x => x.Code == request.TourRequest.Code);
        if (existingTourByCode != null)
        {
            throw new ArgumentException(string.Format(Message.AlreadyExists, "Mã tour"));
        }

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        var tour = _mapper.Map<Tour>(request.TourRequest);

        tour.Rating = 0;
        tour.TotalBookings = 0;
        tour.ViewCount = 0;
        tour.InterestCount = 0;
        tour.CreatedAt = DateTime.UtcNow;

        var imageMain = request.TourRequest.ImageMain;
        var images = request.TourRequest.Images;

        string? mainImageUrl = null;
        var newImageList = new List<Image>();

        if (imageMain != null)
        {
            if (!allowedTypes.Contains(imageMain.ContentType))
            {
                throw new ArgumentException(Message.InvalidImage);
            }

            mainImageUrl = await _fileStorageService.UploadFileAsync(imageMain.OpenReadStream());
        }

        if (images != null && images.Count > 0)
        {
            foreach (var img in images)
            {
                if (!allowedTypes.Contains(img.ContentType))
                {
                    throw new ArgumentException(Message.InvalidImage);
                }
            }

            foreach (var img in images)
            {
                var fileUrl = await _fileStorageService.UploadFileAsync(img.OpenReadStream());
                newImageList.Add(new Image
                {
                    EntityType = Domain.Enums.EntityType.Tour,
                    Url = fileUrl
                });
            }
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            if (mainImageUrl != null)
            {
                tour.ImageMainUrl = mainImageUrl;
            }

            await _unitOfWork.Tours.AddAsync(tour, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            foreach (var img in newImageList)
            {
                img.EntityId = tour.Id;
            }

            if (newImageList.Count > 0)
            {
                await _unitOfWork.Images.AddRangeAsync(newImageList, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            // Xóa các file đã tải lên trong trường hợp lỗi
            if (mainImageUrl != null)
            {
                await _fileStorageService.DeleteFileAsync(mainImageUrl);
            }
            foreach (var img in newImageList)
            {
                await _fileStorageService.DeleteFileAsync(img.Url);
            }

            throw;
        }

        _logger.LogInformation("Tour created with ID: {TourId}", tour.Id);
        return _mapper.Map<TourDTO>(tour);
    }
}
#endregion
