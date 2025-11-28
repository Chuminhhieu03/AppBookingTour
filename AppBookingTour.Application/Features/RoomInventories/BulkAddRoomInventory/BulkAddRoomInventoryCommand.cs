using MediatR;

namespace AppBookingTour.Application.Features.RoomInventories.BulkAddRoomInventory
{
    public record BulkAddRoomInventoryCommand(BulkAddRoomInventoryRequest? Request)
        : IRequest<BulkAddRoomInventoryResponse>;
}


