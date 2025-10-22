using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.RoomTypes.AddNewRoomType
{
    public class AddNewRoomTypeDTO
    {
        public int AccommodationId { get; set; }
        public string Name { get; set; } = null!;
        public int? Type { get; set; }
        public int Capacity { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
    public class AddNewRoomTypeResponse : BaseResponse
    {
        public RoomType? RoomType { get; set; }
    }
}
