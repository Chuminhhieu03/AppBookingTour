using AutoMapper;

using AppBookingTour.Domain.Entities;
using AppBookingTour.Application.Features.Combos.CreateCombo;
using AppBookingTour.Application.Features.Combos.GetComboById;

namespace AppBookingTour.Application.Features.Combos.Mapping;

public class ComboProfile : Profile
{
    public ComboProfile()
    {
        CreateMap<ComboRequestDTO, Combo>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Combo, ComboDTO>()
            .ForMember(dest => dest.FromCityName, opt => opt.MapFrom(src => src.FromCity.Name))
            .ForMember(dest => dest.ToCityName, opt => opt.MapFrom(src => src.ToCity.Name))
            .ForMember(dest => dest.Vehicle, opt => opt.MapFrom(src => src.Vehicle.ToString()))
            .ForMember(dest => dest.HotelImages, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.HotelImages)
                ? new List<string>()
                : src.HotelImages.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()))
            .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Amenities)
                ? new List<string>()
                : src.Amenities.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()))
            .ForMember(dest => dest.Includes, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Includes)
                ? new List<string>()
                : src.Includes.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()))
            .ForMember(dest => dest.Excludes, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Excludes)
                ? new List<string>()
                : src.Excludes.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()));
    }
}