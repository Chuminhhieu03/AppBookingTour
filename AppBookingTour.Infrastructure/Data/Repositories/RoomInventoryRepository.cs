using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.IRepositories;
using AppBookingTour.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppBookingTour.Infrastructure.Data.Repositories
{
    public class RoomInventoryRepository : Repository<RoomInventory>, IRoomInventoryRepository
    {
        public RoomInventoryRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<RoomInventory>> GetByRoomTypeAndDateRange(int roomTypeId, DateTime fromDate, DateTime toDateExclusive)
        {
            return await _dbSet
                .Where(ri => ri.RoomTypeId == roomTypeId&& ri.Date >= fromDate && ri.Date < toDateExclusive)
                .OrderBy(ri => ri.Date)
                .ToListAsync();
        }

        public async Task<List<RoomInventory>> GetByRoomTypeId(int roomTypeId)
        {
            IQueryable<RoomInventory> query = _dbSet;
            query = query.Where(x  => x.RoomTypeId == roomTypeId && x.BookedRooms > 0);
            return await query.ToListAsync();
        }

        public async Task<List<RoomInventory>> SearchRoomInventory(int? roomTypeId, DateTime? date, int? minQuantity, int pageIndex, int pageSize)
        {
            IQueryable<RoomInventory> query = _dbSet;
            if (roomTypeId.HasValue)
                query = query.Where(x => x.RoomTypeId == roomTypeId.Value);
            if (date.HasValue)
                query = query.Where(x => x.Date == date.Value);
            query = query.OrderBy(x => x.Id).Skip(pageIndex * pageSize).Take(pageSize);
            return await query.ToListAsync();
        }
    }
}
