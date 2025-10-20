using AppBookingTour.Application.Features.Discounts.AddNewDiscount;
using AppBookingTour.Application.Features.Discounts.UpdateDiscount;
using AppBookingTour.Domain.Entities;
using AutoMapper;

namespace AppBookingTour.Application.Features.Discounts.Mapping
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            #region Discount mapping

            CreateMap<AddNewDiscountDTO, Discount>();

            CreateMap<UpdateDiscountDTO, Discount>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            #endregion
        }
    }
}
