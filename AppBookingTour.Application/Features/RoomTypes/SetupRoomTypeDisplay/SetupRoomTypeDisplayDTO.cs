using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.RoomTypes.SetupRoomTypeDisplay 
{ 
    public class SetupRoomTypeDisplayDTO : BaseResponse
    {
        public RoomType? RoomType { get; set; }
    }
}
