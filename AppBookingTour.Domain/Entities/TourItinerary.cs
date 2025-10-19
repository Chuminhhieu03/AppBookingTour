namespace AppBookingTour.Domain.Entities;

public class TourItinerary : BaseEntity
{
    public int TourId { get; set; }
    public int DayNumber { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Activity { get; set; }
    public string? Note { get; set; }

    // Navigation properties
    public virtual Tour Tour { get; set; } = null!;
    public virtual ICollection<TourItineraryDestination> Destinations { get; set; } = [];
}