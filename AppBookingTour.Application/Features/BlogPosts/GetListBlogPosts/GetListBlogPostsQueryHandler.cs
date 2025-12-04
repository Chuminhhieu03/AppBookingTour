using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.BlogPosts.GetListBlogPosts;

public class GetListBlogPostsQueryHandler : IRequestHandler<GetListBlogPostsQuery, PagedResult<BlogPostListDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetListBlogPostsQueryHandler> _logger;

    public GetListBlogPostsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetListBlogPostsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedResult<BlogPostListDto>> Handle(GetListBlogPostsQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        _logger.LogInformation("Getting list of blog posts with filters: Status={Status}, CityId={CityId}, AuthorId={AuthorId}",
            request.Status, request.CityId, request.AuthorId);

        //var blogPostRepo = _unitOfWork.Repository<Domain.Entities.BlogPost>() as IBlogPostRepository;
        
        //if (blogPostRepo == null)
        //{
        //    // Fallback to generic repository if cast fails
        //    _logger.LogWarning("IBlogPostRepository not available, using generic repository");
        //    var (items, totalCount) = await _unitOfWork.Repository<Domain.Entities.BlogPost>()
        //        .GetPagedAsync(
        //            request.PageIndex,
        //            request.PageSize,
        //            predicate: null,
        //            orderBy: b => b.CreatedAt,
        //            descending: true,
        //            cancellationToken);

        //    var dtos = items.Select(b => new BlogPostListDto
        //    {
        //        Id = b.Id,
        //        Title = b.Title,
        //        Slug = b.Slug,
        //        AuthorName = "Unknown",
        //        CityName = null,
        //        Status = b.Status,
        //        StatusName = b.Status.ToString(),
        //        PublishedDate = b.PublishedDate,
        //        Tags = b.Tags,
        //        CreatedAt = b.CreatedAt
        //    }).ToList();

        //    return new PagedResult<BlogPostListDto>
        //    {
        //        Items = dtos,
        //        TotalCount = totalCount,
        //        PageIndex = request.PageIndex,
        //        PageSize = request.PageSize
        //    };
        //}

        // Use specialized repository method
        var (blogPosts, total) = await _unitOfWork.BlogPosts.GetPagedWithFiltersAsync(
            request.PageIndex,
            request.PageSize,
            request.Status,
            request.CityId,
            request.AuthorId,
            request.SearchTerm,
            cancellationToken);

        var blogPostDtos = blogPosts.Select(b => new BlogPostListDto
        {
            Id = b.Id,
            Title = b.Title,
            Slug = b.Slug,
            AuthorName = b.Author?.FullName ?? b.Author?.UserName ?? "Unknown",
            CityName = b.City?.Name,
            Status = b.Status,
            StatusName = b.Status.ToString(),
            PublishedDate = b.PublishedDate,
            Tags = b.Tags,
            CoverImage = b.CoverImage,
            Description = b.Description,
            CreatedAt = b.CreatedAt
        }).ToList();

        return new PagedResult<BlogPostListDto>
        {
            Items = blogPostDtos,
            TotalCount = total,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }
}
