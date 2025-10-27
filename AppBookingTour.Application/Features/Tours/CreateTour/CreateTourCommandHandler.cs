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
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };

        var tour = _mapper.Map<Tour>(request.TourRequest);
        tour.Rating = 0;
        tour.TotalBookings = 0;
        tour.ViewCount = 0;
        tour.InterestCount = 0;
        tour.CreatedAt = DateTime.UtcNow;

        var imageMain = request.TourRequest.ImageMain;
        var images = request.TourRequest.Images;

        if (imageMain != null)
        {
            if (!allowedTypes.Contains(imageMain?.ContentType))
                throw new ArgumentException(Message.InvalidImage);

            var fileUrl = await _fileStorageService.UploadFileAsync(imageMain!.OpenReadStream());
            tour.ImageMainUrl = fileUrl;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        await _unitOfWork.Tours.AddAsync(tour, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var listImage = new List<Image>();
        if (images != null && images.Count > 0)
        {
            foreach (var item in images)
            {
                if (!allowedTypes.Contains(item?.ContentType))
                    throw new ArgumentException(Message.InvalidImage);
                var fileUrl = await _fileStorageService.UploadFileAsync(item.OpenReadStream());
                var image = new Image
                {
                    EntityId = tour.Id,
                    EntityType = Domain.Enums.EntityType.Tour,
                    Url = fileUrl
                };
                listImage.Add(image);
            }
        }
        await _unitOfWork.Images.AddRangeAsync(listImage, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        var tourDto = _mapper.Map<TourDTO>(tour);

        return tourDto;
    }
}
#endregion
