using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Domain.Entities;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;

namespace AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;
#region Handler
public sealed class CreateTourItineraryCommandHandler : IRequestHandler<CreateTourItineraryCommand, CreateTourItineraryResponse>
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
    public async Task<CreateTourItineraryResponse> Handle(CreateTourItineraryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Create tour itinerary from dto request");
        try
        {
            var tourItinerary = _mapper.Map<TourItinerary>(request.TourItineraryRequest);
            tourItinerary.CreatedAt = DateTime.UtcNow;

            //TODO: làm thế nào để dùng _unitOfWork.TourItinerary ???
            await _unitOfWork.Repository<TourItinerary>().AddAsync(tourItinerary, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Tour itinerary created successfully with ID: {TourItineraryId}", tourItinerary.Id);

            var tourItineraryDto = _mapper.Map<TourItineraryDTO>(tourItinerary);

            return CreateTourItineraryResponse.Success(tourItineraryDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating tour itinerary");
            return CreateTourItineraryResponse.Failed("An error occurred while creating the tour itinerary.");
        }
    }
}
#endregion