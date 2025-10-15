namespace AppBookingTour.Domain.Entities;

public class TourItineraryDestination : BaseEntity
{
    public int TourItineraryId { get; set; }
    // Removed Destination link; keep basic fields or repurpose as simple POI text
    public int Order { get; set; }
    public TimeSpan? ArrivalTime { get; set; }
    public TimeSpan? DepartureTime { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public virtual TourItinerary TourItinerary { get; set; } = null!;
}