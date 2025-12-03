using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.GetAccommodationForBooking;

/// <summary>
/// Query to get accommodation with room type and selected room inventories for booking
/// Frontend has already selected specific room inventories (multiple nights)
/// </summary>
/// <param name="RoomInventoryIds">List of room inventory IDs that user selected</param>
public record GetAccommodationForBookingQuery(
    List<int> RoomInventoryIds
) : IRequest<AccommodationForBookingDTO?>;
