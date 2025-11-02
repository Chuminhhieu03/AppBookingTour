using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.Features.TourDepartures.GetListTourDeparture;

public sealed class GetTourDeparturesByTourIdQueryHandler : IRequestHandler<GetTourDeparturesByTourIdQuery, List<ListTourDepartureItem>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetTourDeparturesByTourIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetTourDeparturesByTourIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetTourDeparturesByTourIdQueryHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<ListTourDepartureItem>> Handle(GetTourDeparturesByTourIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all tour departures for TourId: {TourId}", request.TourId);

        var tourExists = await _unitOfWork.Repository<Tour>().ExistsAsync(t => t.Id == request.TourId, cancellationToken);
        if (!tourExists)
        {
            _logger.LogWarning("Tour with ID: {TourId} not found when fetching departures", request.TourId);
            throw new KeyNotFoundException($"Tour with ID {request.TourId} not found.");
        }

        var departures = await _unitOfWork.Repository<TourDeparture>()
                                    .FindAsync(predicate: d => d.TourId == request.TourId, cancellationToken);

        var departureDtos = _mapper.Map<List<ListTourDepartureItem>>(departures);
        departureDtos = departureDtos.OrderBy(d => d.DepartureDate).ToList();

        return departureDtos;
    }
}