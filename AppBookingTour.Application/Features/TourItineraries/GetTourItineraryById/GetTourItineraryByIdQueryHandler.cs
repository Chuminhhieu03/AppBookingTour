using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;

#region Handler
public sealed class GetTourItineraryByIdQueryHandler : IRequestHandler<GetTourItineraryByIdQuery, TourItineraryDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetTourItineraryByIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    public GetTourItineraryByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetTourItineraryByIdQueryHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<TourItineraryDTO> Handle(GetTourItineraryByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting tour itinerary details for ID: {TourItineraryId}", request.TourItineraryId);

        var itinerary = await _unitOfWork.Repository<TourItinerary>().GetByIdAsync(request.TourItineraryId, cancellationToken);

        if (itinerary == null)
        {
            _logger.LogWarning("Tour itinerary not found with ID: {TourItineraryId}", request.TourItineraryId);
            throw new KeyNotFoundException($"Tour itinerary with ID {request.TourItineraryId} not found.");
        }

        _logger.LogInformation("Successfully retrieved itinerary for ID: {TourItineraryId}", request.TourItineraryId);
        var itineraryDto = _mapper.Map<TourItineraryDTO>(itinerary);

        return itineraryDto;
    }
}
#endregion