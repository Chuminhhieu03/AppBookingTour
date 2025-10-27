using AppBookingTour.Application.Features.SystemParameters.CreateSystemParameter;
using AppBookingTour.Application.Features.SystemParameters.GetSystemParameterById;
using AppBookingTour.Domain.Entities;
using AutoMapper;

namespace AppBookingTour.Application.Features.SystemParameters.Mapping
{
    public class SystemParameterProfile : Profile
    {
        public SystemParameterProfile()
        {
            CreateMap<SystemParameterRequestDTO, SystemParameter>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<SystemParameter, SystemParameterDTO>();
        }
    }
}