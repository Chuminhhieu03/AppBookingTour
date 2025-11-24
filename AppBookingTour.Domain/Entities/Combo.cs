using AppBookingTour.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppBookingTour.Domain.Entities;

public class Combo : BaseEntity
{
    public int FromCityId { get; set; }
    public int ToCityId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? ShortDescription { get; set; }
    public Vehicle Vehicle { get; set; }
    public int DurationDays { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChildren { get; set; }
    public string? Amenities { get; set; } // JSON
    public string? Description { get; set; } // TEXT
    public string? Includes { get; set; } // JSON
    public string? Excludes { get; set; } // JSON
    public string? TermsConditions { get; set; } // JSON
    [Column(TypeName = "nvarchar(max)")]
    public string? AdditionalInfo { get; set; }
    [Column(TypeName = "nvarchar(max)")]
    public string? ImportantInfo { get; set; }
    public decimal Rating { get; set; }
    public int TotalBookings { get; set; }
    public int ViewCount { get; set; }
    public int InterestCount { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ComboImageCoverUrl { get; set; }
    [NotMapped]
    public List<Image>? Images { get; set; } 

    // Navigation properties
    public virtual City FromCity { get; set; } = null!;
    public virtual City ToCity { get; set; } = null!;
    public virtual ICollection<ComboSchedule> Schedules { get; set; } = [];
    public virtual ICollection<Review> Reviews { get; set; } = [];
    public virtual ICollection<Booking> Bookings { get; set; } = [];
}