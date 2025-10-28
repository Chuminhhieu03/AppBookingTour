using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.RoomTypes.SetupRoomTypeDisplay
{
    public record class SetupRoomTypeDisplayQuery(int id) : IRequest<SetupRoomTypeDisplayDTO>;

}
