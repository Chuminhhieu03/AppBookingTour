using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Domain.IRepositories
{
    public interface IRoomInventoryRepository : IRepository<RoomInventory>
    {
        Task<List<RoomInventory>> SearchRoomInventory(int? roomTypeId, DateTime? date, int? minQuantity, int pageIndex, int pageSize);
    }
}
