using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.RoomTypes.GetPreviewRoomTypeById
{
    public class PreviewRoomTypeDTO : BaseResponse
    {
        public RoomType? RoomType { get; set; }
        public Accommodation? Accommodation { get; set; }
    }
}
