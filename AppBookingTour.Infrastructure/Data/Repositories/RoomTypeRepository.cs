using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.IRepositories;
using AppBookingTour.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AppBookingTour.Infrastructure.Data.Repositories
{
    public class RoomTypeRepository : Repository<RoomType>, IRoomTypeRepository
    {
        public RoomTypeRepository(ApplicationDbContext context) : base(context) { }
        public async Task<List<RoomType>> SearchRoomType(string? name, int? type, int? accommodationId, int pageIndex, int pageSize)
        {
            IQueryable<RoomType> query = _dbSet;
            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.Name.Contains(name));
            query = query.OrderBy(x => x.Id).Skip(pageIndex * pageSize).Take(pageSize);
            return await query.ToListAsync();
        }

        public async Task<RoomType?> GetById(int id)
        {
            IQueryable<RoomType> query = _dbSet;
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<RoomType>> GetByAccommodationId(int accommodationId)
        {
            IQueryable<RoomType> query = _dbSet
                .Include(x => x.ListRoomInventory);
            return await query.Where(e => e.AccommodationId == accommodationId).ToListAsync();
        }
    }
}
