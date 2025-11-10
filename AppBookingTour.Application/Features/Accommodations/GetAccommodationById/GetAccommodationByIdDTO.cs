using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.Accommodations.GetAccommodationById
{
    public class GetAccommodationByIdDTO : BaseResponse
    {
        public Accommodation? Accommodation { get; set; }
    }
}
