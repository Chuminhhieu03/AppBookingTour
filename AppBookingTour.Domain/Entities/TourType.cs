using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Domain.Entities;

public class TourType : BaseEntity
{
    public string Name { get; set; } = null!;
    public PriceLevel? PriceLevel { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Tour> Tours { get; set; } = [];
}

