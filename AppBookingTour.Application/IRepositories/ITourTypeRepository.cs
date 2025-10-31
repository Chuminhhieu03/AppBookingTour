using AppBookingTour.Application.Features.TourTypes.SearchTourType;

using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.IRepositories;

public interface ITourTypeRepository : IRepository<TourType>
{
    Task<(List<TourType> TourTypes, int TotalCount)> SearchTourTypesAsync(
        TourTypeFilter filter,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default);
}