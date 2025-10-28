
namespace AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;

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