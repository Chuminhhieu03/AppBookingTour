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
            CreateMap<AddNewRoomTypeDTO, RoomType>();
            CreateMap<UpdateRoomTypeDTO, RoomType>();
        }
    }
}
