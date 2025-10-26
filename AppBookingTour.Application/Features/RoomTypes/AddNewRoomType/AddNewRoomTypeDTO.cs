using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;
using Microsoft.AspNetCore.Http;

namespace AppBookingTour.Application.Features.RoomTypes.AddNewRoomType
{
    public class AddNewRoomTypeDTO
    {
        public int AccommodationId { get; set; }
        public string Name { get; set; } = null!;
        public int? Type { get; set; }
        public int? MaxAdult { get; set; }
        public int? MaxChildren { get; set; }
        public int? Status { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? ExtraAdultPrice { get; set; }
        public decimal? ExtraChildrenPrice { get; set; }
        public IFormFile? CoverImgFile { get; set; }
        public List<IFormFile>? InfoImgFile { get; set; }
    }

    public class AddNewRoomTypeResponse : BaseResponse
    {
        public RoomType? RoomType { get; set; }
    }
}
