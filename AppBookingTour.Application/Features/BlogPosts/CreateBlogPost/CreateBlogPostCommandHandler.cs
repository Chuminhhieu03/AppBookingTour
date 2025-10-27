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
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<CreateBlogPostCommandHandler> _logger;

    public CreateBlogPostCommandHandler(
        IUnitOfWork unitOfWork,
        IHtmlSanitizerService htmlSanitizer,
        ICurrentUserService currentUserService,
        UserManager<User> userManager,
        ILogger<CreateBlogPostCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _htmlSanitizer = htmlSanitizer;
        _currentUserService = currentUserService;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<CreateBlogPostResponse> Handle(CreateBlogPostCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        _logger.LogInformation("Creating blog post with title: {Title}", request.Title);

        // Get current user ID from JWT token
        var currentUserId = _currentUserService.GetCurrentUserId();
        if (!currentUserId.HasValue)
        {
            return new CreateBlogPostResponse
            {
                Success = false,
                Message = "Không thể xác định người dùng hiện tại. Vui lòng đăng nhập lại"
            };
        }

        // Verify author exists in database
        var author = await _userManager.FindByIdAsync(currentUserId.Value.ToString());
        if (author == null)
        {
            _logger.LogWarning("User with ID {UserId} not found in database", currentUserId.Value);
            return new CreateBlogPostResponse
            {
                Success = false,
                Message = "Tác giả không tồn tại trong hệ thống"
            };
        }

        // Verify city exists if provided
        if (request.CityId.HasValue)
        {
            var cityExists = await _unitOfWork.Repository<City>().ExistsAsync(c => c.Id == request.CityId.Value, cancellationToken);
            if (!cityExists)
            {
                return new CreateBlogPostResponse
                {
                    Success = false,
                    Message = "Thành phố không tồn tại trong hệ thống"
                };
            }
        }

        // Check slug uniqueness
        var slugExists = await _unitOfWork.BlogPosts.IsSlugExistsAsync(request.Slug, null, cancellationToken);
        if (slugExists)
        {
            return new CreateBlogPostResponse
            {
                Success = false,
                Message = "Slug đã tồn tại. Vui lòng chọn slug khác"
            };
        }

        // Sanitize HTML content before saving
        var sanitizedContent = _htmlSanitizer.Sanitize(request.Content);

        var blogPost = new BlogPost
        {
            AuthorId = currentUserId.Value,
            CityId = request.CityId,
            Title = request.Title,
            Content = sanitizedContent,
            Slug = request.Slug,
            Status = request.Status,
            Tags = request.Tags,
            PublishedDate = request.Status == BlogStatus.Published ? DateTime.UtcNow : null,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.BlogPosts.AddAsync(blogPost, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Blog post created successfully with ID: {BlogPostId} by user {UserId}", 
            blogPost.Id, currentUserId.Value);

        return new CreateBlogPostResponse
        {
            Success = true,
            Message = "Tạo bài viết thành công",
            BlogPostId = blogPost.Id
        };
    }
}
