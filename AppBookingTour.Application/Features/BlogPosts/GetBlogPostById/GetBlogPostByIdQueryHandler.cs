using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.BlogPosts.GetBlogPostById;

public class GetBlogPostByIdQueryHandler : IRequestHandler<GetBlogPostByIdQuery, BlogPostDetailDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetBlogPostByIdQueryHandler> _logger;

    public GetBlogPostByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetBlogPostByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BlogPostDetailDto?> Handle(GetBlogPostByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting blog post with ID: {BlogPostId}", request.Id);

        var blogPost = await _unitOfWork.Repository<BlogPost>()
            .GetByIdAsync(request.Id, b => b.Author, b => b.City);

        if (blogPost == null)
        {
            _logger.LogWarning("Blog post not found with ID: {BlogPostId}", request.Id);
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
            CreatedAt = blogPost.CreatedAt,
            UpdatedAt = blogPost.UpdatedAt
        };
    }
}
