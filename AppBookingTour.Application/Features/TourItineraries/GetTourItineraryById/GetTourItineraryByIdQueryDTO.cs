
namespace AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;

public class GetTourItineraryByIdResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public TourItineraryDTO? TourItinerary { get; init; }

    public static GetTourItineraryByIdResponse Success(TourItineraryDTO tourItinerary) =>
        new() { IsSuccess = true, TourItinerary = tourItinerary };

    public static GetTourItineraryByIdResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}
public class TourItineraryDTO
{
    public int Id { get; set; }
    public int TourId { get; set; }
    public int DayNumber { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Activity { get; set; }
    public string? Note { get; set; }
}