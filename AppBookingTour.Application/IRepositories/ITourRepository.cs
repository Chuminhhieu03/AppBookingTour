using AppBookingTour.Application.Features.Tours.SearchTours;
using AppBookingTour.Application.Features.Tours.SearchToursForCustomer;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.IRepositories;

public interface ITourRepository : IRepository<Tour>
{
    Task<Tour?> GetTourWithDetailsAsync(int tourId, CancellationToken cancellationToken = default);
    Task<(List<Tour> Tours, int TotalCount)> SearchToursAsync(SearchTourFilter filter, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
    Task<(List<Tour> Tours, int TotalCount)> SearchToursForCustomerAsync(SearchToursForCustomerFilter filter, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
    Task<List<Tour>> GetFeaturedToursAsync(int count, CancellationToken cancellationToken = default);
}