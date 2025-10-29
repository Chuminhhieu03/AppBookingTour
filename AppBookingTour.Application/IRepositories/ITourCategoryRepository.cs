using AppBookingTour.Application.Features.TourCategories.SearchTourCategory;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.IRepositories;

public interface ITourCategoryRepository : IRepository<TourCategory>
{
    Task<(List<TourCategory> Categories, int TotalCount)> SearchTourCategoriesAsync(
        TourCategoryFilter filter,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default);
}