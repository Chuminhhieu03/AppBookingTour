using MediatR;

namespace AppBookingTour.Application.Features.RoomTypes.SetupRoomTypeAddnew
{
    public record class SetupRoomTypeAddnewQuery() : IRequest<SetupRoomTypeAddnewDTO>;

}
