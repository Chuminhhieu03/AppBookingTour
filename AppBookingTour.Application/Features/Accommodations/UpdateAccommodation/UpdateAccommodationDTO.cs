using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;
using Microsoft.AspNetCore.Http;

namespace AppBookingTour.Application.Features.Accommodations.UpdateAccommodation
{
    public class UpdateAccommodationDTO
    {
        public int CityId { get; set; }
        public int? Type { get; set; }
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public int StarRating { get; set; }
        public decimal? Rating { get; set; }
        public string? Description { get; set; }
        public string? Regulation { get; set; }
        public string? Amenities { get; set; }
        public bool IsActive { get; set; } = true;
        public IFormFile? CoverImgFile { get; set; }
        public IFormFile? InfoImgFile { get; set; }
    }

    public class UpdateAccommodationResponse : BaseResponse
    {
        public Accommodation? Accommodation { get; set; }
    }
}
