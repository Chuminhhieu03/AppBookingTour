using MediatR;

namespace AppBookingTour.Application.Features.RoomTypes.UpdateRoomType
{
    public record UpdateRoomTypeCommand(int RoomTypeId, UpdateRoomTypeDTO RoomType) : IRequest<UpdateRoomTypeResponse>;
}
