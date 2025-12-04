using MediatR;

namespace AppBookingTour.Application.Features.RoomInventories.AddNewRoomInventory
{
    public record AddNewRoomInventoryCommand(AddNewRoomInventoryDTO RoomInventory)
        : IRequest<AddNewRoomInventoryResponse>;
}


