using AppBookingTour.Domain.Enums;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.BlogPosts.GetListBlogPosts;

public class GetListBlogPostsRequest : PaginationRequest
{
    public BlogStatus? Status { get; set; }
    public int? CityId { get; set; }
    public int? AuthorId { get; set; }
    public string? SearchTerm { get; set; }
}

public class BlogPostListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public string? CityName { get; set; }
    public BlogStatus Status { get; set; }
    public string StatusName { get; set; } = null!;
    public DateTime? PublishedDate { get; set; }
    public string? Tags { get; set; }
    public string? CoverImage { get; set; }
    public DateTime CreatedAt { get; set; }
}
