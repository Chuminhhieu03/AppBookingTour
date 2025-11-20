using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Domain.Entities;

public class  BlogPost : BaseEntity
{
    public int AuthorId { get; set; }
    public int? CityId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public BlogStatus Status { get; set; } = BlogStatus.Draft;
    public DateTime? PublishedDate { get; set; }
    public string? Tags { get; set; } // JSON
    public string? CoverImage { get; set; }

    // Navigation properties
    public virtual User Author { get; set; } = null!;
    public virtual City? City { get; set; }
}