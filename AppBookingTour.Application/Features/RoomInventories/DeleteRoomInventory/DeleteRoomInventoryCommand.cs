using MediatR;

namespace AppBookingTour.Application.Features.RoomInventories.DeleteRoomInventory;

public sealed record DeleteRoomInventoryCommand(int Id) : IRequest<DeleteRoomInventoryResponse>;

