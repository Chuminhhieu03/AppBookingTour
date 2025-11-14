using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.RoomInventories.UpdateRoomInventory
{
    public class UpdateRoomInventoryDTO
    {
        public int RoomTypeId { get; set; }
        public DateTime Date { get; set; }
        public decimal? BasePriceAdult { get; set; }
        public decimal? BasePriceChildren { get; set; }
    }

    public class UpdateRoomInventoryResponse : BaseResponse
    {
        public RoomInventory? RoomInventory { get; set; }
    }
}
