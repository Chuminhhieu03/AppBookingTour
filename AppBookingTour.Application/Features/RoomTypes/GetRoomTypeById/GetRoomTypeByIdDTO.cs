using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.RoomTypes.GetRoomTypeById
{
    public class GetRoomTypeByIdDTO : BaseResponse
    {
        public RoomType? RoomType { get; set; }
    }
}

