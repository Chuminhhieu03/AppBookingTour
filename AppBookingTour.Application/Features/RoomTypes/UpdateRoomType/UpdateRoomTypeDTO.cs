using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.RoomTypes.UpdateRoomType
{
    public class UpdateRoomTypeDTO
    {
        public int AccommodationId { get; set; }
        public string Name { get; set; } = null!;
        public int? Type { get; set; }
        public int Capacity { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateRoomTypeResponse : BaseResponse
    {
        public RoomType? RoomType { get; set; }
    }
}
