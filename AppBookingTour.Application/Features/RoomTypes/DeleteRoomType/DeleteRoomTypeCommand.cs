using MediatR;

namespace AppBookingTour.Application.Features.RoomTypes.DeleteRoomType;

public sealed record DeleteRoomTypeCommand(int Id) : IRequest<DeleteRoomTypeResponse>;

