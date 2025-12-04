using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.RoomInventories.BulkDeleteRoomInventory
{
    public class BulkDeleteRoomInventoryRequest
    {
        public IReadOnlyCollection<int> Ids { get; set; } = Array.Empty<int>();
    }

    public class BulkDeleteRoomInventoryResponse : BaseResponse
    {
        public int DeletedCount { get; set; }
        public IReadOnlyCollection<RoomInventory>? DeletedInventories { get; set; }
    }
}


