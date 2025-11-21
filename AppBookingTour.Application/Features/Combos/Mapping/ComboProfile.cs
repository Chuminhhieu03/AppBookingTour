using AppBookingTour.Application.Features.Combos.CreateCombo;
using AppBookingTour.Application.Features.Combos.GetComboById;
using AppBookingTour.Application.Features.Combos.SearchCombosForCustomer;
using AppBookingTour.Domain.Entities;
using AutoMapper;

namespace AppBookingTour.Application.Features.Combos.Mapping;

public class ComboProfile : Profile
{
    public ComboProfile()
    {
        CreateMap<ComboRequestDTO, Combo>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Combo, ComboDTO>()
            .ForMember(dest => dest.FromCityId, opt => opt.MapFrom(src => src.FromCity.Id))
            .ForMember(dest => dest.ToCityId, opt => opt.MapFrom(src => src.ToCity.Id))
            .ForMember(dest => dest.FromCityName, opt => opt.MapFrom(src => src.FromCity.Name))
            .ForMember(dest => dest.ToCityName, opt => opt.MapFrom(src => src.ToCity.Name))
            .ForMember(dest => dest.Vehicle, opt => opt.MapFrom(src => src.Vehicle.ToString()))
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

        CreateMap<Combo, CustomerComboListItem>()
        .ForMember(dest => dest.FromCityName, opt => opt.MapFrom(src => src.FromCity != null ? src.FromCity.Name : "N/A"))
        .ForMember(dest => dest.ToCityName, opt => opt.MapFrom(src => src.ToCity != null ? src.ToCity.Name : "N/A"))
        .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src =>
            src.Schedules.Where(s => s.Status == Domain.Enums.ComboStatus.Available)
                         .OrderBy(s => s.DepartureDate)
            ));
    }
}