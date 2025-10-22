using MediatR;

namespace AppBookingTour.Application.Features.RoomInventories.SearchRoomInventories
{
    public record class SearchRoomInventoryQuery(int? pageIndex, int? pageSize, SearchRoomInventoryFilter? roomInventoryFilter) : IRequest<SearchRoomInventoryResponse>;
}
