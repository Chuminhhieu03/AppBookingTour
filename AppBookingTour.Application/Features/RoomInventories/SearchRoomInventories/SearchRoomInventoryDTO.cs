using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.RoomInventories.SearchRoomInventories
{
    public class SearchRoomInventoryResponse : BaseResponse
    {
        public List<RoomInventory>? ListRoomInventory { get; set; }
    }
    public class SearchRoomInventoryFilter
    {
        public int? RoomTypeId { get; set; }
        public DateTime? Date { get; set; }
        public int? MinQuantity { get; set; }
    }
}
