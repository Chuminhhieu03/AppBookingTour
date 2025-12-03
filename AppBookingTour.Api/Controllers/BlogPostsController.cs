using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.BlogPosts.CreateBlogPost;
using AppBookingTour.Application.Features.BlogPosts.DeleteBlogPost;
using AppBookingTour.Application.Features.BlogPosts.GetBlogPostById;
using AppBookingTour.Application.Features.BlogPosts.GetBlogPostBySlug;
using AppBookingTour.Application.Features.BlogPosts.GetListBlogPosts;
using AppBookingTour.Application.Features.BlogPosts.GetRandomBlogTitles;
using AppBookingTour.Application.Features.BlogPosts.UpdateBlogPost;
using AppBookingTour.Share.DTOS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers;

/// <summary>
/// BlogPost management controller with HTML sanitization security
/// </summary>
[ApiController]
[Route("api/blogposts")]
public class BlogPostsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<BlogPostsController> _logger;

    public BlogPostsController(IMediator mediator, ILogger<BlogPostsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Create new blog post (Admin/Staff only)
    /// </summary>
    [HttpPost]
    //[Authorize(Roles = "Admin,Staff")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ApiResponse<CreateBlogPostResponse>>> CreateBlogPost([FromForm] CreateBlogPostRequest request)
    {
        var result = await _mediator.Send(new CreateBlogPostCommand(request));

        if (!result.Success)
        {
            return BadRequest(ApiResponse<CreateBlogPostResponse>.Fail(result.Message));
        }

        return Ok(ApiResponse<CreateBlogPostResponse>.Ok(result));
    }

    /// <summary>
    /// Update existing blog post (Admin/Staff only)
    /// </summary>
    [HttpPut("{id}")]
    //[Authorize(Roles = "Admin,Staff")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ApiResponse<UpdateBlogPostResponse>>> UpdateBlogPost(int id, [FromForm] UpdateBlogPostRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest(ApiResponse<UpdateBlogPostResponse>.Fail("ID không khớp"));
        }

        var result = await _mediator.Send(new UpdateBlogPostCommand(request));

        if (!result.Success)
        {
            return BadRequest(ApiResponse<UpdateBlogPostResponse>.Fail(result.Message));
        }

        return Ok(ApiResponse<UpdateBlogPostResponse>.Ok(result));
    }

    /// <summary>
    /// Delete blog post (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<DeleteBlogPostResponse>>> DeleteBlogPost(int id)
    {
        var result = await _mediator.Send(new DeleteBlogPostCommand(id));

        if (!result.Success)
        {
            return BadRequest(ApiResponse<DeleteBlogPostResponse>.Fail(result.Message));
        }

        return Ok(ApiResponse<DeleteBlogPostResponse>.Ok(result));
    }

    /// <summary>
    /// Get blog post by ID (Public)
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<BlogPostDetailDto>>> GetBlogPostById(int id)
    {
        var result = await _mediator.Send(new GetBlogPostByIdQuery(id));

        if (result == null)
        {
            return NotFound(ApiResponse<BlogPostDetailDto>.Fail("Bài viết không tồn tại"));
        }

        return Ok(ApiResponse<BlogPostDetailDto>.Ok(result));
    }

    /// <summary>
    /// Get blog post by slug (Public - SEO friendly)
    /// </summary>
    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<ApiResponse<BlogPostDetailDto>>> GetBlogPostBySlug(string slug)
    {
        var result = await _mediator.Send(new GetBlogPostBySlugQuery(slug));

        if (result == null)
        {
            return NotFound(ApiResponse<BlogPostDetailDto>.Fail("Bài viết không tồn tại"));
        }

        return Ok(ApiResponse<BlogPostDetailDto>.Ok(result));
    }

    /// <summary>
    /// Get list of blog posts with filters and pagination (Public)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<BlogPostListDto>>>> GetListBlogPosts(
        [FromQuery] GetListBlogPostsRequest request)
    {
        var result = await _mediator.Send(new GetListBlogPostsQuery(request));

        return Ok(ApiResponse<Share.DTOS.PagedResult<BlogPostListDto>>.Ok(result));
    }

    /// <summary>
    /// Get random blog titles (Public)
    /// </summary>
    [HttpGet("random-titles")]
    public async Task<ActionResult<ApiResponse<List<BlogTitleDto>>>> GetRandomBlogTitles([FromQuery] int count = 5)
    {
        if (count <= 0 || count > 100)
        {
            return BadRequest(ApiResponse<List<BlogTitleDto>>.Fail("Số lượng phải từ 1 đến 100"));
        }

        var result = await _mediator.Send(new GetRandomBlogTitlesQuery(count));

        return Ok(ApiResponse<List<BlogTitleDto>>.Ok(result));
    }
}
