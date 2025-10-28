using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.RoomTypes.SetupRoomTypeAddnew;

public class SetupRoomTypeAddnewDTO : BaseResponse
{
    public List<KeyValuePair<int, string>>? ListStatus { get; set; }
    public List<SystemParameter>? ListAmenity { get; set; }
}
