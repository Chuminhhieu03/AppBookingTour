using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.RoomInventories.AddNewRoomInventory
{
    public class AddNewRoomInventoryDTO
    {
        public int RoomTypeId { get; set; }
        public DateTime Date { get; set; }
        public decimal BasePrice { get; set; }
        public int BookedRooms { get; set; }
        public decimal? BasePriceAdult { get; set; }
        public decimal? BasePriceChildren { get; set; }
    }

    public class AddNewRoomInventoryResponse : BaseResponse
    {
        public RoomInventory? RoomInventory { get; set; }
    }
}


