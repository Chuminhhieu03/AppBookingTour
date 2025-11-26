using AppBookingTour.Application.Features.TourDepartures.CreateTourDeparture;
using AppBookingTour.Application.Features.Tours.GetTourById;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
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

        // 1. Validate nghiệp vụ
        var existingTourByCode = await _unitOfWork.Tours.FirstOrDefaultAsync(x => x.Code == request.TourRequest.Code);
        if (existingTourByCode != null)
        {
            throw new ArgumentException(string.Format(Message.AlreadyExists, "Mã tour"));
        }

        if (request.TourRequest.Departures != null && request.TourRequest.Departures.Any())
        {
            await ValidateTourDeparturesAsync(request.TourRequest.Departures, cancellationToken);
        }

        // 2. Upload hình ảnh
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        var imageMain = request.TourRequest.ImageMain;
        var images = request.TourRequest.Images;

        if (imageMain != null && !allowedTypes.Contains(imageMain.ContentType))
            throw new ArgumentException(Message.InvalidImage);

        if (images != null && images.Any(img => !allowedTypes.Contains(img.ContentType)))
            throw new ArgumentException(Message.InvalidImage);

        string? mainImageUrl = null;
        var newImageList = new List<Image>();

        try
        {
            if (imageMain != null)
                mainImageUrl = await _fileStorageService.UploadFileAsync(imageMain.OpenReadStream());

            if (images != null && images.Count > 0)
            {
                foreach (var img in images)
                {
                    var fileUrl = await _fileStorageService.UploadFileAsync(img.OpenReadStream());
                    newImageList.Add(new Image { EntityType = EntityType.Tour, Url = fileUrl });
                }
            }
        }
        catch
        {
            if (mainImageUrl != null) await _fileStorageService.DeleteFileAsync(mainImageUrl);
            foreach (var img in newImageList) await _fileStorageService.DeleteFileAsync(img.Url);
            throw;
        }

        // 3. Lưu dữ liệu (Transaction)
        var tour = _mapper.Map<Tour>(request.TourRequest);
        tour.Rating = 0;
        tour.TotalBookings = 0;
        tour.ViewCount = 0;
        tour.InterestCount = 0;
        tour.CreatedAt = DateTime.UtcNow;

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            if (mainImageUrl != null)
            {
                tour.ImageMainUrl = mainImageUrl;
            }

            await _unitOfWork.Tours.AddAsync(tour, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Gán ID & Lưu thư viện ảnh
            foreach (var img in newImageList) img.EntityId = tour.Id;
            if (newImageList.Count > 0)
            {
                await _unitOfWork.Images.AddRangeAsync(newImageList, cancellationToken);
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            if (mainImageUrl != null) await _fileStorageService.DeleteFileAsync(mainImageUrl);
            foreach (var img in newImageList) await _fileStorageService.DeleteFileAsync(img.Url);
            throw;
        }

        _logger.LogInformation("Tạo tour thành công với ID: {TourId}", tour.Id);
        return _mapper.Map<TourDTO>(tour);
    }

    private async Task ValidateTourDeparturesAsync(List<TourDepartureRequestDTO> departures, CancellationToken cancellationToken)
    {
        var departuresWithGuide = departures
            .Where(x => x.GuideId.HasValue && x.DepartureDate.HasValue && x.ReturnDate.HasValue)
            .ToList();

        if (!departuresWithGuide.Any()) return;

        var guideIds = departuresWithGuide.Select(x => x.GuideId!.Value).Distinct().ToList();

        // Lấy Map (Id -> Name) để check tồn tại và lấy tên hiển thị lỗi
        var validGuidesMap = await _unitOfWork.Profiles.GetGuideNamesMapAsync(guideIds, cancellationToken);

        if (validGuidesMap.Count != guideIds.Count)
        {
            throw new ArgumentException("Danh sách chứa hướng dẫn viên không tồn tại hoặc không hợp lệ.");
        }

        var requestGroupedByGuide = departuresWithGuide.GroupBy(x => x.GuideId!.Value);
        foreach (var group in requestGroupedByGuide)
        {
            var sortedDeps = group.OrderBy(x => x.DepartureDate).ToList();
            for (int i = 0; i < sortedDeps.Count - 1; i++)
            {
                if (sortedDeps[i].ReturnDate > sortedDeps[i + 1].DepartureDate)
                {
                    throw new ArgumentException($"Hướng dẫn viên '{validGuidesMap[group.Key]}' bị trùng lịch ngay trong danh sách gửi lên.");
                }
            }
        }

        var minDate = departuresWithGuide.Min(x => x.DepartureDate!.Value);
        var maxDate = departuresWithGuide.Max(x => x.ReturnDate!.Value);

        var busySchedules = await _unitOfWork.Repository<TourDeparture>()
            .FindAsync(x => guideIds.Contains(x.GuideId!.Value)
                            && x.Status != DepartureStatus.Cancelled
                            && x.DepartureDate < maxDate
                            && x.ReturnDate > minDate,
                       cancellationToken);

        var busyScheduleLookup = busySchedules.ToLookup(x => x.GuideId!.Value);

        foreach (var dep in departuresWithGuide)
        {
            var guideSpecificSchedules = busyScheduleLookup[dep.GuideId!.Value];
            if (guideSpecificSchedules.Any(x => x.DepartureDate < dep.ReturnDate && x.ReturnDate > dep.DepartureDate))
            {
                throw new ArgumentException($"Hướng dẫn viên '{validGuidesMap[dep.GuideId!.Value]}' đã bị trùng lịch với Tour khác vào ngày {dep.DepartureDate:dd/MM/yyyy}.");
            }
        }
    }
}
#endregion