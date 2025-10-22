using MediatR;

namespace AppBookingTour.Application.Features.RoomInventories.UpdateRoomInventory
{
    public record UpdateRoomInventoryCommand(int RoomInventoryId, UpdateRoomInventoryDTO RoomInventory) : IRequest<UpdateRoomInventoryResponse>;
}
