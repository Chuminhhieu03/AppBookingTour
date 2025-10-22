using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using MediatR;

namespace AppBookingTour.Application.Features.RoomTypes.SearchRoomTypes
{
    public class SearchRoomTypeHandler : IRequestHandler<SearchRoomTypeQuery, SearchRoomTypeResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public SearchRoomTypeHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<SearchRoomTypeResponse> Handle(SearchRoomTypeQuery request, CancellationToken cancellationToken)
        {
            var filter = request.roomTypeFilter ?? new SearchRoomTypeFilter();
            int pageIndex = request.pageIndex ?? Constants.Pagination.PageIndex;
            int pageSize = request.pageSize ?? Constants.Pagination.PageSize;
            var list = await _unitOfWork.RoomTypes.SearchRoomType(filter.Name, filter.Type, filter.AccommodationId, pageIndex, pageSize);
            return new SearchRoomTypeResponse { Success = true, ListRoomType = list };
        }
    }
}
