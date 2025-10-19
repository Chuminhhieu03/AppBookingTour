using AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;

namespace AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;

public class CreateTourItineraryResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public TourItineraryDTO? TourItinerary { get; init; }
    public static CreateTourItineraryResponse Success(TourItineraryDTO tourItinerary) =>
        new() { IsSuccess = true, TourItinerary = tourItinerary };
    public static CreateTourItineraryResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}

public class TourItineraryRequestDTO
{
    public int? TourId { get; set; }
    public int? DayNumber { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Activity { get; set; }
    public string? Note { get; set; }
}
