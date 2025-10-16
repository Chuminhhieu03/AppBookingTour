

namespace AppBookingTour.Application.Features.TourItineraries.DeleteTourItinerary;

public class DeleteTourItineraryResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public static DeleteTourItineraryResponse Success() =>
        new() { IsSuccess = true };
    public static DeleteTourItineraryResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}
