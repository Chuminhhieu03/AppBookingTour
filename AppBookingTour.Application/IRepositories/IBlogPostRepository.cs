using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
using System.Linq.Expressions;

namespace AppBookingTour.Application.IRepositories;

public interface IBlogPostRepository : IRepository<BlogPost>
{
    Task<BlogPost?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<bool> IsSlugExistsAsync(string slug, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<(IEnumerable<BlogPost> Items, int TotalCount)> GetPagedWithFiltersAsync(
        int pageIndex,
        int pageSize,
        BlogStatus? status,
        int? cityId,
        int? authorId,
        string? searchTerm,
        CancellationToken cancellationToken = default);
    Task UpdateCoverImageAsync(int blogPostId, string? coverImageUrl, CancellationToken cancellationToken = default);
}
