using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using MediatR;

namespace AppBookingTour.Application.Features.RoomInventories.SearchRoomInventories
{
    public class SearchRoomInventoryHandler : IRequestHandler<SearchRoomInventoryQuery, SearchRoomInventoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public SearchRoomInventoryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<SearchRoomInventoryResponse> Handle(SearchRoomInventoryQuery request, CancellationToken cancellationToken)
        {
            var filter = request.roomInventoryFilter ?? new SearchRoomInventoryFilter();
            int pageIndex = request.pageIndex ?? Constants.Pagination.PageIndex;
            int pageSize = request.pageSize ?? Constants.Pagination.PageSize;
            var list = await _unitOfWork.RoomInventories.SearchRoomInventory(filter.RoomTypeId, filter.Date, filter.MinQuantity, pageIndex, pageSize);
            return new SearchRoomInventoryResponse { Success = true, ListRoomInventory = list };
        }
    }
}
