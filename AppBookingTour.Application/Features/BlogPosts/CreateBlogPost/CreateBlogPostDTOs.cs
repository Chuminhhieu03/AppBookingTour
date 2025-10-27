using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.BlogPosts.CreateBlogPost;

public class CreateBlogPostRequest
{
    //public int AuthorId { get; set; }
    public int? CityId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public BlogStatus Status { get; set; }
    public string? Tags { get; set; }
}

public class CreateBlogPostResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int? BlogPostId { get; set; }
}
