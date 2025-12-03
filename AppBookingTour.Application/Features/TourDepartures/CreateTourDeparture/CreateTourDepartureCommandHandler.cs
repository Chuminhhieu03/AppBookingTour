using AppBookingTour.Application.Features.TourDepartures.GetTourDepartureById;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourDepartures.CreateTourDeparture;

#region Handler
public sealed class CreateTourDepartureCommandHandler : IRequestHandler<CreateTourDepartureCommand, TourDepartureDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateTourDepartureCommandHandler> _logger;
    private readonly IMapper _mapper;
    public CreateTourDepartureCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateTourDepartureCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<TourDepartureDTO> Handle(CreateTourDepartureCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Create tour departure from dto request");

        var tourId = request.TourId;

        // 1. Kiểm tra tour có tồn tại không
        var tour = await _unitOfWork.Tours.GetByIdAsync(tourId);
        if (tour == null)
        {
            throw new KeyNotFoundException($"Tour with ID {tourId} not found.");
        }

        // 2. Kiểm tra ngày khởi hành đã tồn tại cho tour này chưa
        var existingTourDepartureByDate = await _unitOfWork.Repository<TourDeparture>()
            .FirstOrDefaultAsync(x =>
                x.DepartureDate == request.TourDepartureRequest.DepartureDate
                && x.TourId == tourId);
        if (existingTourDepartureByDate != null)
        {
            throw new ArgumentException(string.Format(Message.AlreadyExists, "Ngày khởi hành"));
        }

        // 3. Kiểm tra hướng dẫn viên có tồn tại và không bị trùng lịch
        if (request.TourDepartureRequest.GuideId.HasValue)
        {
            var guideId = request.TourDepartureRequest.GuideId.Value;
            var startDate = request.TourDepartureRequest.DepartureDate!.Value;
            var endDate = request.TourDepartureRequest.ReturnDate!.Value;

            var guideMap = await _unitOfWork.Profiles.GetGuideNamesMapAsync([guideId]);
            if (guideMap.Count == 0)
            {
                throw new ArgumentException("Hướng dẫn viên không tồn tại.");
            }

            var isGuideBusy = await _unitOfWork.Repository<TourDeparture>()
                .ExistsAsync(x =>
                    x.GuideId == guideId
                    && x.Status != DepartureStatus.Cancelled
                    && x.DepartureDate < endDate
                    && x.ReturnDate > startDate
                );

            if (isGuideBusy)
            {
                throw new ArgumentException("Hướng dẫn viên đã có lịch trình trong khoảng thời gian này.");
            }
        }

        // 4. Tạo mới khởi hành
        var tourDeparture = _mapper.Map<TourDeparture>(request.TourDepartureRequest);
        tourDeparture.TourId = tourId;
        tourDeparture.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.Repository<TourDeparture>().AddAsync(tourDeparture, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Tour departure created successfully with ID: {TourDepartureId}", tourDeparture.Id);
        var tourDepartureDto = _mapper.Map<TourDepartureDTO>(tourDeparture);

        return tourDepartureDto;
    }
}
#endregion
