namespace AppBookingTour.Domain.Entities;

public class TourCategory : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? ParentCategoryId { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual TourCategory? ParentCategory { get; set; }
    public virtual ICollection<TourCategory> ChildCategories { get; set; } = [];
    public virtual ICollection<Tour> Tours { get; set; } = [];
}