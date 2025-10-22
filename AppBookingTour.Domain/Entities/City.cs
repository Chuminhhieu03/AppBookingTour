using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Domain.Entities;

public class City : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Code { get; set; }
    public Region? Region { get; set; }
    public bool IsPopular { get; set; } = false;
    public string Slug { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;

    //// Navigation properties
    //public virtual ICollection<Accommodation> Hotels { get; set; } = [];
    public virtual ICollection<Tour> Tours { get; set; } = [];
    public virtual ICollection<BlogPost> BlogPosts { get; set; } = [];
    public virtual ICollection<Combo> FromCombos { get; set; } = [];
    public virtual ICollection<Combo> ToCombos { get; set; } = [];
}

