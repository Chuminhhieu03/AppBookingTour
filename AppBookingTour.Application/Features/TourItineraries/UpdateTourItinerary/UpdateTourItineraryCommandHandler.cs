using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Application.IRepositories;

namespace AppBookingTour.Application.Features.TourItineraries.UpdateTourItinerary;

#region Handler
public sealed class UpdateTourItineraryCommandHandler : IRequestHandler<UpdateTourItineraryCommand, UpdateTourItineraryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateTourItineraryCommandHandler> _logger;
    private readonly IMapper _mapper;
    public UpdateTourItineraryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateTourItineraryCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<UpdateTourItineraryResponse> Handle(UpdateTourItineraryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating tour itinerary with ID: {TourItineraryId}", request.TourItineraryId);
        try
        {
            var existingItinerary = await _unitOfWork.Repository<Domain.Entities.TourItinerary>()
                .GetByIdAsync(request.TourItineraryId, cancellationToken);
            if (existingItinerary == null)
            {
                return UpdateTourItineraryResponse.Failed($"Tour itinerary with ID {request.TourItineraryId} not found.");
            }

            // Map updated fields from request to existing entity
            _mapper.Map(request.TourItineraryRequest, existingItinerary);
            existingItinerary.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Tour itinerary updated with ID: {TourItineraryId}", request.TourItineraryId);
            return UpdateTourItineraryResponse.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour itinerary with ID: {TourItineraryId}", request.TourItineraryId);
            return UpdateTourItineraryResponse.Failed("An error occurred while updating the tour itinerary.");
        }
    }
}
#endregion
