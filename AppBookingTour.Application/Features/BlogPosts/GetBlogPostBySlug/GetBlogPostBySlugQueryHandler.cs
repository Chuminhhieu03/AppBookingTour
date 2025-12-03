using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.Features.BlogPosts.GetBlogPostById;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.BlogPosts.GetBlogPostBySlug;

public class GetBlogPostBySlugQueryHandler : IRequestHandler<GetBlogPostBySlugQuery, BlogPostDetailDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetBlogPostBySlugQueryHandler> _logger;

    public GetBlogPostBySlugQueryHandler(IUnitOfWork unitOfWork, ILogger<GetBlogPostBySlugQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BlogPostDetailDto?> Handle(GetBlogPostBySlugQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting blog post with slug: {Slug}", request.Slug);

        var blogPostRepo = _unitOfWork.BlogPosts;
        
        if (blogPostRepo == null)
        {
            _logger.LogWarning("IBlogPostRepository not available");
            return null;
        }

        var blogPost = await blogPostRepo.GetBySlugAsync(request.Slug, cancellationToken);

        if (blogPost == null)
        {
            _logger.LogWarning("Blog post not found with slug: {Slug}", request.Slug);
            return null;
        }

        return new BlogPostDetailDto
        {
            Id = blogPost.Id,
            AuthorId = blogPost.AuthorId,
            AuthorName = blogPost.Author?.FullName ?? blogPost.Author?.UserName ?? "Unknown",
            CityId = blogPost.CityId,
            CityName = blogPost.City?.Name,
            Title = blogPost.Title,
            Content = blogPost.Content,
            Slug = blogPost.Slug,
            Status = blogPost.Status,
            StatusName = blogPost.Status.ToString(),
            PublishedDate = blogPost.PublishedDate,
            Tags = blogPost.Tags,
            Description = blogPost.Description,
            CoverImage = blogPost.CoverImage,
            CreatedAt = blogPost.CreatedAt,
            UpdatedAt = blogPost.UpdatedAt
        };
    }
}
