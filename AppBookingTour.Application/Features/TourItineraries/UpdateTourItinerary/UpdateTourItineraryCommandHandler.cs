using AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;
using AppBookingTour.Application.IRepositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourItineraries.UpdateTourItinerary;

#region Handler
public sealed class UpdateTourItineraryCommandHandler : IRequestHandler<UpdateTourItineraryCommand, TourItineraryDTO>
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
    public async Task<TourItineraryDTO> Handle(UpdateTourItineraryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating tour itinerary with ID: {TourItineraryId}", request.TourItineraryId);
        var existingItinerary = await _unitOfWork.Repository<Domain.Entities.TourItinerary>().GetByIdAsync(request.TourItineraryId, cancellationToken);
        if (existingItinerary == null)
        {
            throw new KeyNotFoundException($"Tour itinerary with ID {request.TourItineraryId} not found.");
        }

        _mapper.Map(request.TourItineraryRequest, existingItinerary);
        existingItinerary.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Tour itinerary updated with ID: {TourItineraryId}", request.TourItineraryId);

        var updatedItineraryDto = _mapper.Map<TourItineraryDTO>(existingItinerary);

        return updatedItineraryDto;
    }
}
#endregion
