using AppBookingTour.Application.Features.RoomInventories.AddNewRoomInventory;
using AppBookingTour.Application.Features.RoomInventories.UpdateRoomInventory;
using AppBookingTour.Domain.Entities;
using AutoMapper;

namespace AppBookingTour.Application.Features.RoomInventories.Mapping
{
    public class RoomInventoryProfile : Profile
    {
        public RoomInventoryProfile()
        {
            CreateMap<AddNewRoomInventoryDTO, RoomInventory>();
            CreateMap<UpdateRoomInventoryDTO, RoomInventory>();
        }
    }
}
