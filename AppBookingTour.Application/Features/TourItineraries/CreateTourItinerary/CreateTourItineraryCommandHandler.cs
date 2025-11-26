using AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;
#region Handler
public sealed class CreateTourItineraryCommandHandler : IRequestHandler<CreateTourItineraryCommand, TourItineraryDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateTourItineraryCommandHandler> _logger;
    private readonly IMapper _mapper;
    public CreateTourItineraryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateTourItineraryCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<TourItineraryDTO> Handle(CreateTourItineraryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Create tour itinerary from dto request");

        var tourId = request.TourId;

        var tour = await _unitOfWork.Tours.GetByIdAsync(tourId);
        if (tour == null)
        {
            throw new KeyNotFoundException($"Tour with ID {tourId} not found.");
        }

        var existingTourItineraryByName = await _unitOfWork.Repository<TourItinerary>()
            .FirstOrDefaultAsync(x =>
                x.DayNumber == request.TourItineraryRequest.DayNumber
                && x.TourId == tourId);
        if (existingTourItineraryByName != null)
        {
            throw new ArgumentException(string.Format(Message.AlreadyExists, "Ngày lịch trình"));
        }

        var tourItinerary = _mapper.Map<TourItinerary>(request.TourItineraryRequest);
        tourItinerary.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.Repository<TourItinerary>().AddAsync(tourItinerary, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Tour itinerary created successfully with ID: {TourItineraryId}", tourItinerary.Id);
        var tourItineraryDto = _mapper.Map<TourItineraryDTO>(tourItinerary);

        return tourItineraryDto;
    }
}
#endregion