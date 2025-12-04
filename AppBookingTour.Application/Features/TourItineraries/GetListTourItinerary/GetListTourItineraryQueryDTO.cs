
namespace AppBookingTour.Application.Features.TourItineraries.GetListTourItinerary;

public class TourItineraryListItem
{
    public int Id { get; set; }
    public int DayNumber { get; set; }
    public string Title { get; set; } = null!;
    public string? Activity { get; set; }
}
