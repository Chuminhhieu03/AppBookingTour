namespace AppBookingTour.Domain.Entities;

public class TourItinerary : BaseEntity
{
    public int TourId { get; set; }
    public string DayNumber { get; set; } = null!; // VARCHAR(50) ?? h? tr? "Day 1", "Day 2-3", etc.
    public string Title { get; set; } = null!; // Thêm Title property
    public string? Description { get; set; }
    public string? Activity { get; set; } // TEXT
    public string? Note { get; set; }
    public int Order { get; set; } // To order itineraries within a tour

    // Navigation properties
    public virtual Tour Tour { get; set; } = null!;
    public virtual ICollection<TourItineraryDestination> Destinations { get; set; } = [];
}