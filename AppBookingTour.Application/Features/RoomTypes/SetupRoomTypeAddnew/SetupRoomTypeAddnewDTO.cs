using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.RoomTypes.SetupRoomTypeAddnew;

public class SetupRoomTypeAddnewDTO : BaseResponse
{
    public List<KeyValuePair<int, string>>? ListStatus { get; set; }
}
