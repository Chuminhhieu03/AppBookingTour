using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.Features.TourItineraries.DeleteTourItinerary;

#region Handler
public sealed class DeleteTourItineraryCommandHandler : IRequestHandler<DeleteTourItineraryCommand, DeleteTourItineraryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteTourItineraryCommandHandler> _logger;
    private readonly IMapper _mapper;
    public DeleteTourItineraryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteTourItineraryCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<DeleteTourItineraryResponse> Handle(DeleteTourItineraryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting tour itinerary with ID: {TourItineraryId}", request.TourItineraryId);
        try
        {
            var tourItinerary = await _unitOfWork.Repository<TourItinerary>().GetByIdAsync(request.TourItineraryId, cancellationToken);
            if (tourItinerary == null)
            {
                _logger.LogWarning("Tour itinerary with ID: {TourItineraryId} not found", request.TourItineraryId);
                return DeleteTourItineraryResponse.Failed("Tour itinerary not found.");
            }

            _unitOfWork.Repository<TourItinerary>().Remove(tourItinerary);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Tour itinerary with ID: {TourItineraryId} deleted successfully", request.TourItineraryId);
            return DeleteTourItineraryResponse.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting tour itinerary with ID: {TourItineraryId}", request.TourItineraryId);
            return DeleteTourItineraryResponse.Failed("An error occurred while deleting the tour itinerary.");
        }
    }
}
#endregion