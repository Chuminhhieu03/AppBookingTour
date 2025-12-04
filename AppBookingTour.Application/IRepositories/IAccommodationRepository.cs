using AppBookingTour.Application.Features.Accommodations.SearchAccommodation;
using AppBookingTour.Application.Features.Accommodations.SearchAccommodationsForCustomer;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.IRepositories
{
    public interface IAccommodationRepository : IRepository<Accommodation>
    {
        Task<(List<Accommodation> ListAccommodation, int TotalCount)> SearchAccommodation(SearchAccommodationFilter accommodationFilter, int pageIndex, int pageSize);
        Task<Accommodation> GetById(int id);
        Task<(List<CustomerAccommodationListItem> Results, int TotalCount)> SearchAccommodationsForCustomerAsync(
            SearchAccommodationsForCustomerFilter filter, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
    }
}
