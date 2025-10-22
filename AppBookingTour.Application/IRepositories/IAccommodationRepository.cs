using AppBookingTour.Application.Features.Accommodations.SearchAccommodation;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.IRepositories
{
    public interface IAccommodationRepository : IRepository<Accommodation>
    {
        Task<List<Accommodation>> SearchAccommodation(SearchAccommodationFilter accommodationFilter, int pageIndex, int pageSize);
    }
}
