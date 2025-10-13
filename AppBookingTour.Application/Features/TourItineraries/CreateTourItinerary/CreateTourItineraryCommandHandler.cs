using MediatR;
using Microsoft.Extensions.Logging;
using AppBookingTour.Domain.Entities;

using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;

namespace AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;
#region Handler
public sealed class CreateTourItineraryCommandHandler : IRequestHandler<CreateTourItineraryCommand, CreateTourItineraryCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateTourItineraryCommandHandler> _logger;
    public CreateTourItineraryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateTourItineraryCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<CreateTourItineraryCommandResponse> Handle(CreateTourItineraryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Create tour itinerary from dto request");
        try
        {
            // TODO: dùng AutoMapper để map
            // map từ DTO => entity
            var tourItinerary = new TourItinerary
            {
                TourId = request.TourItineraryRequest.TourId!.Value,
                DayNumber = request.TourItineraryRequest.DayNumber!.Value,
                Title = request.TourItineraryRequest.Title!,
                Description = request.TourItineraryRequest.Description,
                Activity = request.TourItineraryRequest.Activity,
                Note = request.TourItineraryRequest.Note,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };
            await _unitOfWork.Repository<TourItinerary>().AddAsync(tourItinerary, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Tour itinerary created successfully with ID: {TourItineraryId}", tourItinerary.Id);

            // TODO: dùng AutoMapper để map
            // map từ entity => dto
            var tourItineraryDto = new TourItineraryDto
            {
                Id = tourItinerary.Id,
                TourId = tourItinerary.TourId,
                DayNumber = tourItinerary.DayNumber,
                Title = tourItinerary.Title,
                Description = tourItinerary.Description,
                Activity = tourItinerary.Activity,
                Note = tourItinerary.Note
            };
            return CreateTourItineraryCommandResponse.Success(tourItineraryDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating tour itinerary");
            return CreateTourItineraryCommandResponse.Failed("An error occurred while creating the tour itinerary.");
        }
    }
}
#endregion