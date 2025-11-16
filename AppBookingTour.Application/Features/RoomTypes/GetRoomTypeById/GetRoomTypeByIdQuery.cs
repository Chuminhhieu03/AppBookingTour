using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.RoomTypes.GetRoomTypeById
{
    public record class GetRoomTypeByIdQuery(int id) : IRequest<GetRoomTypeByIdDTO>;

}

