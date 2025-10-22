using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Infrastructure.Database;

namespace AppBookingTour.Infrastructure.Data.Repositories
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(ApplicationDbContext context) : base(context) { }
    }
}
