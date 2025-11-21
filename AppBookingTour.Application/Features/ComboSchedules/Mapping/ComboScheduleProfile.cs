using AppBookingTour.Application.Features.Combos.SearchCombosForCustomer;
using AppBookingTour.Application.Features.ComboSchedules.CreateComboSchedule;
using AppBookingTour.Application.Features.ComboSchedules.GetComboScheduleById;
using AppBookingTour.Domain.Entities;
using AutoMapper;

namespace AppBookingTour.Application.Features.ComboSchedules.Mapping
{
    public class ComboScheduleProfile : Profile
    {
        public ComboScheduleProfile()
        {
            #region ComboSchedule mapping
            CreateMap<ComboScheduleRequestDTO, ComboSchedule>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ComboSchedule, ComboScheduleDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<ComboSchedule, CustomerComboScheduleItem>();
            #endregion
        }
    }
}