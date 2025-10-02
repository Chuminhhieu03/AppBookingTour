namespace AppBookingTour.Domain.Entities;

public class Business : BaseEntity
{
    public int UserId { get; set; }
    public string BusinessName { get; set; } = null!;
    public string? ContactPerson { get; set; }
    public string? TaxCode { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public BusinessStatus Status { get; set; } = BusinessStatus.Pending;

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<Tour> Tours { get; set; } = [];
    public virtual ICollection<Promotion> Promotions { get; set; } = [];
}

public enum BusinessStatus
{
    Active = 1,
    Inactive = 2,
    Pending = 3
}