using System.ComponentModel.DataAnnotations.Schema;

namespace AppBookingTour.Domain.Entities;

public class Tour : BaseEntity
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int TypeId { get; set; }
    public int CategoryId { get; set; }
    public int DepartureCityId { get; set; }
    public int DestinationCityId { get; set; }
    public int DurationDays { get; set; }
    public int DurationNights { get; set; }
    public int MaxParticipants { get; set; }
    public int MinParticipants { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChild { get; set; }
    public string? Description { get; set; }
    [Column(TypeName = "nvarchar(max)")]
    public string? AdditionalInfo { get; set; }
    [Column(TypeName = "nvarchar(max)")]
    public string? ImportantInfo { get; set; }
    public decimal Rating { get; set; }
    public int TotalBookings { get; set; }
    public int ViewCount { get; set; }
    public int InterestCount { get; set; }
    public string? ImageMainUrl { get; set; }
    public bool IsActive { get; set; } = true;
    [NotMapped]
    public List<Image>? Images { get; set; }

    // Navigation properties
    public virtual TourType Type { get; set; } = null!;
    public virtual TourCategory Category { get; set; } = null!;
    public virtual City DepartureCity { get; set; } = null!;
    public virtual City DestinationCity { get; set; } = null!;
    public virtual ICollection<TourDeparture> Departures { get; set; } = [];
    public virtual ICollection<TourItinerary> Itineraries { get; set; } = [];
}
