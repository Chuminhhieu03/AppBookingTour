using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.BlogPosts.UpdateBlogPost;

public class UpdateBlogPostCommandHandler : IRequestHandler<UpdateBlogPostCommand, UpdateBlogPostResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHtmlSanitizerService _htmlSanitizer;
    private readonly ILogger<UpdateBlogPostCommandHandler> _logger;

    public UpdateBlogPostCommandHandler(
        IUnitOfWork unitOfWork,
        IHtmlSanitizerService htmlSanitizer,
        ILogger<UpdateBlogPostCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _htmlSanitizer = htmlSanitizer;
        _logger = logger;
    }

    public async Task<UpdateBlogPostResponse> Handle(UpdateBlogPostCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        _logger.LogInformation("Updating blog post with ID: {BlogPostId}", request.Id);

        var blogPost = await _unitOfWork.Repository<BlogPost>().GetByIdAsync(request.Id, cancellationToken);
        if (blogPost == null)
        {
            return new UpdateBlogPostResponse
            {
                Success = false,
                Message = "Bài vi?t không t?n t?i"
            };
        }

        // Verify city exists if provided
        if (request.CityId.HasValue)
        {
            var cityExists = await _unitOfWork.Repository<City>().ExistsAsync(c => c.Id == request.CityId.Value, cancellationToken);
            if (!cityExists)
            {
                return new UpdateBlogPostResponse
                {
                    Success = false,
                    Message = "Thành ph? không t?n t?i trong h? th?ng"
                };
            }
        }

        // Check slug uniqueness (exclude current blog post)
        if (blogPost.Slug != request.Slug)
        {
            var slugExists = await _unitOfWork.Repository<BlogPost>()
                .ExistsAsync(b => b.Slug == request.Slug && b.Id != request.Id, cancellationToken);

            if (slugExists)
            {
                return new UpdateBlogPostResponse
                {
                    Success = false,
                    Message = "Slug ?ã t?n t?i. Vui lòng ch?n slug khác"
                };
            }
        }

        // ?? CRITICAL: Sanitize HTML content before updating
        var sanitizedContent = _htmlSanitizer.Sanitize(request.Content);

        // Update properties
        blogPost.CityId = request.CityId;
        blogPost.Title = request.Title;
        blogPost.Content = sanitizedContent; // ? Sanitized HTML
        blogPost.Slug = request.Slug;
        blogPost.Status = request.Status;
        blogPost.Tags = request.Tags;
        blogPost.UpdatedAt = DateTime.UtcNow;

        // Set published date if status changed to Published
        if (request.Status == BlogStatus.Published && !blogPost.PublishedDate.HasValue)
        {
            blogPost.PublishedDate = DateTime.UtcNow;
        }

        _unitOfWork.Repository<BlogPost>().Update(blogPost);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Blog post updated successfully with ID: {BlogPostId}", request.Id);

        return new UpdateBlogPostResponse
        {
            Success = true,
            Message = "C?p nh?t bài vi?t thành công"
        };
    }
}
