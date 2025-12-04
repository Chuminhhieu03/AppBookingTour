using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Tours.GetTourForBooking;

/// <summary>
/// Handler for GetTourForBooking - Get tour with selected departure for booking
/// </summary>
public class GetTourForBookingQueryHandler
    : IRequestHandler<GetTourForBookingQuery, TourForBookingDTO?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetTourForBookingQueryHandler> _logger;

    public GetTourForBookingQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetTourForBookingQueryHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TourForBookingDTO?> Handle(
        GetTourForBookingQuery request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Getting tour for booking with departure ID: {DepartureId}",
            request.DepartureId
        );

        // Get the selected departure
        var departure = await _unitOfWork
            .Repository<TourDeparture>()
            .GetByIdAsync(request.DepartureId, cancellationToken);

        if (departure == null)
        {
            _logger.LogWarning("TourDeparture not found: {DepartureId}", request.DepartureId);
            return null;
        }

        // Get tour with navigation properties
        var tour = await _unitOfWork.Tours.GetByIdAsync(departure.TourId, t => t.DepartureCity, t => t.DestinationCity);

        if (tour == null)
        {
            _logger.LogWarning("Tour not found: {TourId}", departure.TourId);
            return null;
        }

        if (!tour.IsActive)
        {
            _logger.LogWarning("Tour is inactive: {TourId}", tour.Id);
            return null;
        }

        // Get guide name if exists
        string? guideName = null;
        if (departure.GuideId.HasValue)
        {
            var guideNames = await _unitOfWork.Profiles.GetGuideNamesMapAsync(
                new[] { departure.GuideId.Value },
                cancellationToken
            );
            guideNames.TryGetValue(departure.GuideId.Value, out guideName);
        }

        _logger.LogInformation(
            "Successfully retrieved tour {TourId} with departure {DepartureId} for booking",
            tour.Id,
            departure.Id
        );

        return new TourForBookingDTO
        {
            Tour = new TourInfoDTO
            {
                Id = tour.Id,
                Code = tour.Code,
                Name = tour.Name,
                ImageMainUrl = tour.ImageMainUrl,
                DurationDays = tour.DurationDays,
                DurationNights = tour.DurationNights,
                Description = tour.Description,
                Rating = tour.Rating,
                TotalBookings = tour.TotalBookings,
                DepartureCityName = tour.DepartureCity.Name,
                DestinationCityName = tour.DestinationCity.Name
            },
            Departure = new TourDepartureInfoDTO
            {
                Id = departure.Id,
                DepartureDate = departure.DepartureDate,
                ReturnDate = departure.ReturnDate,
                PriceAdult = departure.PriceAdult,
                PriceChildren = departure.PriceChildren,
                SingleRoomSurcharge = departure.SingleRoomSurcharge,
                AvailableSlots = departure.AvailableSlots,
                BookedSlots = departure.BookedSlots,
                Status = departure.Status.ToString(),
                GuideName = guideName
            }
        };
    }
}
