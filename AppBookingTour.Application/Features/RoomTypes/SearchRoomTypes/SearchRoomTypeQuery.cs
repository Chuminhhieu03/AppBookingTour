using MediatR;

namespace AppBookingTour.Application.Features.RoomTypes.SearchRoomTypes
{
    public record class SearchRoomTypeQuery(int? pageIndex, int? pageSize, SearchRoomTypeFilter? roomTypeFilter) : IRequest<SearchRoomTypeResponse>;
}
