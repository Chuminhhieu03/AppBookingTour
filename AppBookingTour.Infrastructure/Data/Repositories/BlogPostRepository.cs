using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
using AppBookingTour.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AppBookingTour.Infrastructure.Data.Repositories;

public class BlogPostRepository : Repository<BlogPost>, IBlogPostRepository
{
    private readonly ApplicationDbContext _context;

    public BlogPostRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<BlogPost?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.BlogPosts
            .Include(b => b.Author)
            .Include(b => b.City)
            .FirstOrDefaultAsync(b => b.Slug == slug, cancellationToken);
    }

    public async Task<bool> IsSlugExistsAsync(string slug, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.BlogPosts.Where(b => b.Slug == slug);

        if (excludeId.HasValue)
        {
            query = query.Where(b => b.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<(IEnumerable<BlogPost> Items, int TotalCount)> GetPagedWithFiltersAsync(
        int pageIndex,
        int pageSize,
        BlogStatus? status,
        int? cityId,
        int? authorId,
        string? searchTerm,
        CancellationToken cancellationToken = default)
    {
        var query = _context.BlogPosts
            .Include(b => b.Author)
            .Include(b => b.City)
            .AsQueryable();

        // Apply filters
        if (status.HasValue)
        {
            query = query.Where(b => b.Status == status.Value);
        }

        if (cityId.HasValue)
        {
            query = query.Where(b => b.CityId == cityId.Value);
        }

        if (authorId.HasValue)
        {
            query = query.Where(b => b.AuthorId == authorId.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var search = searchTerm.ToLower();
            query = query.Where(b =>
                b.Title.ToLower().Contains(search) ||
                b.Content.ToLower().Contains(search) ||
                (b.Tags != null && b.Tags.ToLower().Contains(search)));
        }

        // Get total count
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination and sorting
        var items = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task UpdateCoverImageAsync(int blogPostId, string? coverImageUrl, CancellationToken cancellationToken = default)
    {
        var blogPost = await _context.BlogPosts.FindAsync(new object[] { blogPostId }, cancellationToken);
        if (blogPost != null)
        {
            blogPost.CoverImage = coverImageUrl;
            blogPost.UpdatedAt = DateTime.UtcNow;
            _context.BlogPosts.Update(blogPost);
        }
    }
}
