using MediatR;

namespace AppBookingTour.Application.Features.RoomTypes.AddNewRoomType
{
    public record AddNewRoomTypeCommand(AddNewRoomTypeDTO RoomType) : IRequest<AddNewRoomTypeResponse>;
}
