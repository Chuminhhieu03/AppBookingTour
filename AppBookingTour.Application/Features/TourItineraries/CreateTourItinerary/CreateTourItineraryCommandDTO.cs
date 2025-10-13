using AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;

namespace AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;

public class CreateTourItineraryCommandResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public TourItineraryDto? TourItinerary { get; init; }
    public static CreateTourItineraryCommandResponse Success(TourItineraryDto tourItinerary) =>
        new() { IsSuccess = true, TourItinerary = tourItinerary };
    public static CreateTourItineraryCommandResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}

public class TourItineraryRequestDto
{
    public int? TourId { get; set; }
    public int? DayNumber { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Activity { get; set; }
    public string? Note { get; set; }
}
