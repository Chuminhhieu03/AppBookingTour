using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.RoomInventories.AddNewRoomInventory
{
    public class AddNewRoomInventoryDTO
    {
        public int RoomTypeId { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
        public bool IsActive { get; set; } = true;
    }
    public class AddNewRoomInventoryResponse : BaseResponse
    {
        public RoomInventory? RoomInventory { get; set; }
    }
}
