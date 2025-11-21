using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.BlogPosts.GetRandomBlogTitles;

public class GetRandomBlogTitlesQueryHandler : IRequestHandler<GetRandomBlogTitlesQuery, List<BlogTitleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetRandomBlogTitlesQueryHandler> _logger;

    public GetRandomBlogTitlesQueryHandler(IUnitOfWork unitOfWork, ILogger<GetRandomBlogTitlesQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<BlogTitleDto>> Handle(GetRandomBlogTitlesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting {Count} random blog titles", request.Count);

        // Get all published blog posts
        var blogPosts = await _unitOfWork.Repository<BlogPost>()
            .GetAllAsync(cancellationToken);

        if (blogPosts == null || !blogPosts.Any())
        {
            _logger.LogWarning("No published blog posts found");
            return new List<BlogTitleDto>();
        }

        // Get random n blog posts
        var randomBlogPosts = blogPosts
            .OrderBy(x => Guid.NewGuid())
            .Take(request.Count)
            .Select(b => new BlogTitleDto
            {
                Id = b.Id,
                Title = b.Title
            })
            .ToList();

        _logger.LogInformation("Retrieved {Count} random blog titles", randomBlogPosts.Count);

        return randomBlogPosts;
    }
}
