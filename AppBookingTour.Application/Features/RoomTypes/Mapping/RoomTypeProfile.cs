using AppBookingTour.Application.Features.RoomTypes.AddNewRoomType;
using AppBookingTour.Application.Features.RoomTypes.UpdateRoomType;
using AppBookingTour.Domain.Entities;
using AutoMapper;

namespace AppBookingTour.Application.Features.RoomTypes.Mapping
{
    public class RoomTypeProfile : Profile
    {
        public RoomTypeProfile()
        {
            CreateMap<AddNewRoomTypeDTO, RoomType>()
               .ForMember(dest => dest.CheckinHour, opt => opt.Ignore())
                .ForMember(dest => dest.CheckoutHour, opt => opt.Ignore())
                .ForMember(dest => dest.VAT, opt => opt.MapFrom(src => src.VAT))
                .ForMember(dest => dest.ManagementFee, opt => opt.MapFrom(src => src.ManagementFee));

            CreateMap<UpdateRoomTypeDTO, RoomType>()
                .ForMember(dest => dest.CheckinHour, opt => opt.Ignore())
                .ForMember(dest => dest.CheckoutHour, opt => opt.Ignore())
                .ForMember(dest => dest.VAT, opt => opt.MapFrom(src => src.VAT))
                .ForMember(dest => dest.ManagementFee, opt => opt.MapFrom(src => src.ManagementFee));
        }
    }
}
