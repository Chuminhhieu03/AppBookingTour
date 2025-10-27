using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.BlogPosts.CreateBlogPost;

public class CreateBlogPostCommandHandler : IRequestHandler<CreateBlogPostCommand, CreateBlogPostResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHtmlSanitizerService _htmlSanitizer;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<CreateBlogPostCommandHandler> _logger;

    public CreateBlogPostCommandHandler(
        IUnitOfWork unitOfWork,
        IHtmlSanitizerService htmlSanitizer,
        UserManager<User> userManager,
        ILogger<CreateBlogPostCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _htmlSanitizer = htmlSanitizer;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<CreateBlogPostResponse> Handle(CreateBlogPostCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        _logger.LogInformation("Creating blog post with title: {Title}",
            request.Title);

        // Verify author exists using UserManager
        //var author = await _userManager.FindByIdAsync(request.AuthorId.ToString());
        //if (author == null)
        //{
        //    return new CreateBlogPostResponse
        //    {
        //        Success = false,
        //        Message = "Tác gi? không t?n t?i trong h? th?ng"
        //    };
        //}

        // Verify city exists if provided
        //if (request.CityId.HasValue)
        //{
        //    var cityExists = await _unitOfWork.Repository<City>().ExistsAsync(c => c.Id == request.CityId.Value, cancellationToken);
        //    if (!cityExists)
        //    {
        //        return new CreateBlogPostResponse
        //        {
        //            Success = false,
        //            Message = "Thành ph? không t?n t?i trong h? th?ng"
        //        };
        //    }
        //}

        // Check slug uniqueness
        var slugExists = await _unitOfWork.BlogPosts.IsSlugExistsAsync(request.Slug, null, cancellationToken);
        if (slugExists)
        {
            return new CreateBlogPostResponse
            {
                Success = false,
                Message = "Slug ?ã t?n t?i. Vui lòng ch?n slug khác"
            };
        }

        // ?? CRITICAL: Sanitize HTML content before saving
        var sanitizedContent = _htmlSanitizer.Sanitize(request.Content);

        var blogPost = new BlogPost
        {
            AuthorId = 1,
            CityId = 1,
            Title = request.Title,
            Content = sanitizedContent, // ? Sanitized HTML
            Slug = request.Slug,
            Status = request.Status,
            Tags = request.Tags,
            PublishedDate = request.Status == BlogStatus.Published ? DateTime.UtcNow : null,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.BlogPosts.AddAsync(blogPost, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Blog post created successfully with ID: {BlogPostId}", blogPost.Id);

        return new CreateBlogPostResponse
        {
            Success = true,
            Message = "T?o bài vi?t thành công",
            BlogPostId = blogPost.Id
        };
    }
}
