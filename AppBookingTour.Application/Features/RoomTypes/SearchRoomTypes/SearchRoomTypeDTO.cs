using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.RoomTypes.SearchRoomTypes
{
    public class SearchRoomTypeResponse : BaseResponse
    {
        public List<RoomType>? ListRoomType { get; set; }
    }
    public class SearchRoomTypeFilter
    {
        public string? Name { get; set; }
        public int? Type { get; set; }
        public int? AccommodationId { get; set; }
    }
}
