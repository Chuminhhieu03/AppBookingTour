using MediatR;

namespace AppBookingTour.Application.Features.RoomInventories.BulkDeleteRoomInventory
{
    public record BulkDeleteRoomInventoryCommand(BulkDeleteRoomInventoryRequest? Request)
        : IRequest<BulkDeleteRoomInventoryResponse>;
}


