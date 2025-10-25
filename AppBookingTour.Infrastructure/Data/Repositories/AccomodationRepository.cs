using AppBookingTour.Application.Features.Accommodations.SearchAccommodation;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AppBookingTour.Infrastructure.Data.Repositories
{
    public class AccomodationRepository : Repository<Accommodation>, IAccommodationRepository
    {
        public AccomodationRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Accommodation> GetById(int id)
        {
            IQueryable<Accommodation> query = _dbSet.
                Include(x => x.City);
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Accommodation>> SearchAccommodation(SearchAccommodationFilter accommodationFilter, int pageIndex, int pageSize)
        {
            IQueryable<Accommodation> query = _dbSet.Include(x => x.City);
            if (!string.IsNullOrEmpty(accommodationFilter.Name))
                query = query.Where(x => x.Name.Contains(accommodationFilter.Name));
            if (!string.IsNullOrEmpty(accommodationFilter.Code))
                query = query.Where(x => x.Code.Contains(accommodationFilter.Code));
            if (accommodationFilter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == accommodationFilter.IsActive.Value);
            if (accommodationFilter.Type.HasValue)
                query = query.Where(x => x.Type == accommodationFilter.Type.Value);
            if (accommodationFilter.CityId.HasValue)
                query = query.Where(x => x.CityId == accommodationFilter.CityId);
            query = query
                .OrderBy(x => - x.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);
            return await query.ToListAsync();
        }
    }
}
