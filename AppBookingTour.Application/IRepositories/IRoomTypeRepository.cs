using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Domain.IRepositories
{
    public interface IRoomTypeRepository : IRepository<RoomType>
    {
        Task<List<RoomType>> SearchRoomType(string? name, int? type, int? accommodationId, int pageIndex, int pageSize);
        Task<RoomType?> GetById(int id);
        Task<List<RoomType>> GetByAccommodationId(int accommodationId);
    }
}
