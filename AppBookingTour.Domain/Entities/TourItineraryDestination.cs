namespace AppBookingTour.Domain.Entities;

public class TourItineraryDestination : BaseEntity
{
    public int TourItineraryId { get; set; }
    public int DestinationId { get; set; }
    public int Order { get; set; }
    public TimeSpan? ArrivalTime { get; set; }
    public TimeSpan? DepartureTime { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public virtual TourItinerary TourItinerary { get; set; } = null!;
    public virtual Destination Destination { get; set; } = null!;
}