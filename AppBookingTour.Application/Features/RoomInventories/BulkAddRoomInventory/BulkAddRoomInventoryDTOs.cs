using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.RoomInventories.BulkAddRoomInventory
{
    public class BulkAddRoomInventoryRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int BookedRooms { get; set; }
        public decimal BasePrice { get; set; }
        public decimal? BasePriceAdult { get; set; }
        public decimal? BasePriceChildren { get; set; }
        public int RoomTypeId { get; set; }
    }

    public class BulkAddRoomInventoryResponse : BaseResponse
    {
        public IReadOnlyCollection<RoomInventory>? RoomInventories { get; set; }
        public int CreatedRecords => RoomInventories?.Count ?? 0;
    }
}


