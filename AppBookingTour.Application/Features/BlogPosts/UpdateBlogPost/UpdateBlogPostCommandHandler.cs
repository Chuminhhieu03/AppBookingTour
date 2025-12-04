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
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<UpdateBlogPostCommandHandler> _logger;

    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private static readonly string[] AllowedContentTypes = 
    { 
        "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" 
    };
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5MB

    public UpdateBlogPostCommandHandler(
        IUnitOfWork unitOfWork,
        IHtmlSanitizerService htmlSanitizer,
        IFileStorageService fileStorageService,
        ILogger<UpdateBlogPostCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _htmlSanitizer = htmlSanitizer;
        _fileStorageService = fileStorageService;
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

        // Upload new cover image if provided
        string? newCoverImageUrl = null;
        if (request.CoverImageFile != null)
        {
            try
            {
                ValidateImageFile(request.CoverImageFile);

                // Delete old cover image if exists
                if (!string.IsNullOrEmpty(blogPost.CoverImage))
                {
                    try
                    {
                        await _fileStorageService.DeleteFileAsync(blogPost.CoverImage);
                        _logger.LogInformation("Deleted old cover image: {Url}", blogPost.CoverImage);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to delete old cover image: {Url}", blogPost.CoverImage);
                    }
                }

                // Upload new image
                using var stream = request.CoverImageFile.OpenReadStream();
                newCoverImageUrl = await _fileStorageService.UploadFileAsync(stream);
                _logger.LogInformation("Uploaded new cover image for blog post {BlogPostId}: {Url}", 
                    request.Id, newCoverImageUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload cover image for blog post {BlogPostId}", request.Id);
                return new UpdateBlogPostResponse
                {
                    Success = false,
                    Message = $"L?i khi upload ?nh bìa: {ex.Message}"
                };
            }
        }

        // Sanitize HTML content before updating
        var sanitizedContent = _htmlSanitizer.Sanitize(request.Content);

        // Update properties
        blogPost.CityId = request.CityId;
        blogPost.Title = request.Title;
        blogPost.Content = sanitizedContent;
        blogPost.Slug = request.Slug;
        blogPost.Status = request.Status;
        blogPost.Tags = request.Tags;
        blogPost.Description = request.Description;
        
        // Update cover image only if new file was uploaded
        if (newCoverImageUrl != null)
        {
            blogPost.CoverImage = newCoverImageUrl;
        }
        
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
            Message = "C?p nh?t bài vi?t thành công",
            CoverImageUrl = blogPost.CoverImage
        };
    }

    private void ValidateImageFile(Microsoft.AspNetCore.Http.IFormFile file)
    {
        // Validate extension
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException(
                $"??nh d?ng file không h?p l?. Ch? ch?p nh?n: {string.Join(", ", AllowedExtensions)}");
        }

        // Validate content type
        if (!AllowedContentTypes.Contains(file.ContentType.ToLowerInvariant()))
        {
            throw new InvalidOperationException(
                $"Content type không h?p l?. Ch? ch?p nh?n: {string.Join(", ", AllowedContentTypes)}");
        }

        // Validate file size
        if (file.Length > MaxFileSizeBytes)
        {
            throw new InvalidOperationException("Kích th??c file không ???c v??t quá 5MB");
        }

        // Validate file not empty
        if (file.Length == 0)
        {
            throw new InvalidOperationException("File không ???c r?ng");
        }
    }
}
