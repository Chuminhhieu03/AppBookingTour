
namespace AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;

public class TourItineraryRequestDTO
{
    public int? DayNumber { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Activity { get; set; }
    public string? Note { get; set; }
}
