using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.BlogPosts.GetBlogPostById;

public class BlogPostDetailDto
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = null!;
    public int? CityId { get; set; }
    public string? CityName { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public BlogStatus Status { get; set; }
    public string StatusName { get; set; } = null!;
    public DateTime? PublishedDate { get; set; }
    public string? Tags { get; set; }
    public string? Description { get; set; }
    public string? CoverImage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
