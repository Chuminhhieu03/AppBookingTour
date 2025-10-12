using AppBookingTour.Application.Features.Discounts.AddNewDiscount;
using AppBookingTour.Domain.Entities;
using AutoMapper;

namespace AppBookingTour.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Discount mapping

            CreateMap<AddNewDiscountDTO, Discount>();

            #endregion
        }
    }
}
