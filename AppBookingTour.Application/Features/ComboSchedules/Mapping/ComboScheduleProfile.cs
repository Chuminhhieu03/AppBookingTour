using AutoMapper;

using AppBookingTour.Application.Features.ComboSchedules.CreateComboSchedule;
using AppBookingTour.Application.Features.ComboSchedules.GetComboScheduleById;
using AppBookingTour.Domain.Entities;

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
            #endregion
        }
    }
}