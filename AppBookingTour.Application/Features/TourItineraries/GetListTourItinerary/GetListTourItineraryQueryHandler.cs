using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.Features.TourItineraries.GetListTourItinerary;

public sealed class GetTourItinerariesByTourIdQueryHandler : IRequestHandler<GetTourItinerariesByTourIdQuery, List<TourItineraryListItem>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetTourItinerariesByTourIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetTourItinerariesByTourIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetTourItinerariesByTourIdQueryHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<TourItineraryListItem>> Handle(GetTourItinerariesByTourIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all tour itineraries for TourId: {TourId}", request.TourId);
        var tourExists = await _unitOfWork.Repository<Tour>().ExistsAsync(t => t.Id == request.TourId, cancellationToken);
        if (!tourExists)
        {
            _logger.LogWarning("Tour with ID: {TourId} not found", request.TourId);
            throw new KeyNotFoundException($"Tour with ID {request.TourId} not found.");
        }

        var itineraries = await _unitOfWork.Repository<TourItinerary>()
                                    .FindAsync(predicate: i => i.TourId == request.TourId, cancellationToken);

        var tourItineraryItems = _mapper.Map<List<TourItineraryListItem>>(itineraries);

        // Order by DayNumber
        tourItineraryItems = tourItineraryItems.OrderBy(i => i.DayNumber).ToList();

        return tourItineraryItems;
    }
}