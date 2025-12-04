using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;
using Microsoft.AspNetCore.Http;

namespace AppBookingTour.Application.Features.RoomTypes.UpdateRoomType
{
    public class UpdateRoomTypeDTO
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
        public string? CoverImageUrl { get; set; }
        public string? Amenities { get; set; }
        public string? CheckinHour { get; set; } // Format: "HH:mm" or "HH:mm:ss"
        public string? CheckoutHour { get; set; } // Format: "HH:mm" or "HH:mm:ss"
        public decimal? Area { get; set; }
        public string? View { get; set; } // View biển, view đường phố ... List Id
        public string? CancelPolicy { get; set; }
        public IFormFile? CoverImgFile { get; set; }
        public List<int>? ListInfoImageId { get; set; } // Nhận qua Form
        public List<IFormFile>? ListNewInfoImage { get; set; }
        public decimal? VAT { get; set; } // Thuế VAT
        public decimal? ManagementFee { get; set; } // Phụ thu quản trị
    }

    public class UpdateRoomTypeResponse : BaseResponse
    {
        public RoomType? RoomType { get; set; }
    }
}
