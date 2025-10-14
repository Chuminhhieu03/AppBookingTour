namespace AppBookingTour.Domain.Entities;

public class Destination : BaseEntity
{
    public int CityId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? Category { get; set; }
    public decimal? Rating { get; set; }
    public string? ImageGallery { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual City City { get; set; } = null!;
    public virtual ICollection<TourItineraryDestination> TourItineraryDestinations { get; set; } = [];
    public virtual ICollection<Review> Reviews { get; set; } = [];
}