using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.BlogPosts.DeleteBlogPost;

public class DeleteBlogPostCommandHandler : IRequestHandler<DeleteBlogPostCommand, DeleteBlogPostResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteBlogPostCommandHandler> _logger;

    public DeleteBlogPostCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteBlogPostCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<DeleteBlogPostResponse> Handle(DeleteBlogPostCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting blog post with ID: {BlogPostId}", request.Id);

        var blogPost = await _unitOfWork.Repository<BlogPost>().GetByIdAsync(request.Id, cancellationToken);
        if (blogPost == null)
        {
            return new DeleteBlogPostResponse
            {
                Success = false,
                Message = "Bài vi?t không t?n t?i"
            };
        }

        _unitOfWork.Repository<BlogPost>().Remove(blogPost);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Blog post deleted successfully with ID: {BlogPostId}", request.Id);

        return new DeleteBlogPostResponse
        {
            Success = true,
            Message = "Xóa bài vi?t thành công"
        };
    }
}
