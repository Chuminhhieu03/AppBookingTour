
namespace AppBookingTour.Application.Features.TourItineraries.UpdateTourItinerary;

public class UpdateTourItineraryResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public static UpdateTourItineraryResponse Success() =>
        new() { IsSuccess = true };
    public static UpdateTourItineraryResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}
