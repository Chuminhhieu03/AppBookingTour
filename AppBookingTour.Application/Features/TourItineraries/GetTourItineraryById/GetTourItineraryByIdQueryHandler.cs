using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;

#region Handler
public sealed class GetTourItineraryByIdQueryHandler : IRequestHandler<GetTourItineraryByIdQuery, GetTourItineraryByIdResponse>
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
    public async Task<GetTourItineraryByIdResponse> Handle(GetTourItineraryByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting tour itinerary details for ID: {TourItineraryId}", request.TourItineraryId);
        try
        {
            //TODO: làm thế nào để dùng _unitOfWork.TourItinerary ???
            var itinerary = await _unitOfWork.Repository<TourItinerary>().GetByIdAsync(request.TourItineraryId, cancellationToken);

            if (itinerary == null)
            {
                _logger.LogWarning("Tour itinerary not found with ID: {TourItineraryId}", request.TourItineraryId);
                return GetTourItineraryByIdResponse.Failed($"Tour itinerary with ID {request.TourItineraryId} not found");
            }

            var itineraryDto = _mapper.Map<TourItineraryDTO>(itinerary);

            _logger.LogInformation("Successfully retrieved itinerary for ID: {TourItineraryId}", request.TourItineraryId);
            return GetTourItineraryByIdResponse.Success(itineraryDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting tour itinerary with ID: {TourItineraryId}", request.TourItineraryId);
            return GetTourItineraryByIdResponse.Failed("An error occurred while processing your request.");
        }
    }
}
#endregion