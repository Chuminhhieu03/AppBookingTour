using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Domain.Entities;

public class Tour : BaseEntity
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int? BusinessId { get; set; }
    // Removed CategoryId – category now inferred via TourType.Category
    public int TypeId { get; set; }
    public int DepartureCityId { get; set; }
    public string? ImageGallery { get; set; } // JSON
    public int DurationDays { get; set; }
    public int DurationNights { get; set; }
    public int MaxParticipants { get; set; }
    public int MinParticipants { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChild { get; set; }
    public string? Description { get; set; } // TEXT
    public string? Itinerary { get; set; } // JSON
    public string? Includes { get; set; } // JSON
    public string? Excludes { get; set; } // JSON
    public string? TermsConditions { get; set; } // TEXT
    public TourStatus Status { get; set; } = TourStatus.Draft;
    public decimal Rating { get; set; }
    public int TotalBookings { get; set; }
    public int ViewCount { get; set; }
    public int InterestCount { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Business? Business { get; set; }
    // Removed direct Category navigation; resolve via Type.Category
    public virtual TourType Type { get; set; } = null!;
    public virtual City DepartureCity { get; set; } = null!;
    public virtual ICollection<TourDeparture> Departures { get; set; } = [];
    public virtual ICollection<TourItinerary> Itineraries { get; set; } = [];
}
