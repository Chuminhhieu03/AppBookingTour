using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.Accommodations.GetAccommodationForCustomerById
{
    public class GetAccommodationForCustomerByIdDTO : BaseResponse
    {
        public Accommodation? Accommodation { get; set; }
    }
}

