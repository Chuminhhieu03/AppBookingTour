using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.RoomInventories.BulkAddRoomInventory
{
    public class BulkAddRoomInventoryHandler
        : IRequestHandler<BulkAddRoomInventoryCommand, BulkAddRoomInventoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BulkAddRoomInventoryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BulkAddRoomInventoryResponse> Handle(
            BulkAddRoomInventoryCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Request is null)
            {
                return new BulkAddRoomInventoryResponse
                {
                    Success = false,
                    Message = "Thiếu dữ liệu room inventory."
                };
            }

            var payload = request.Request;
            var fromDate = payload.FromDate.Date;
            var toDate = payload.ToDate.Date;

            if (toDate < fromDate)
            {
                return new BulkAddRoomInventoryResponse
                {
                    Success = false,
                    Message = "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu."
                };
            }

            var inventories = new List<RoomInventory>();

            for (var current = fromDate; current <= toDate; current = current.AddDays(1))
            {
                inventories.Add(new RoomInventory
                {
                    RoomTypeId = payload.RoomTypeId,
                    Date = current,
                    BasePrice = payload.BasePrice,
                    BasePriceAdult = payload.BasePriceAdult ?? payload.BasePrice,
                    BasePriceChildren = payload.BasePriceChildren ?? payload.BasePrice,
                    BookedRooms = payload.BookedRooms
                });
            }

            await _unitOfWork.RoomInventories.AddRangeAsync(inventories, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new BulkAddRoomInventoryResponse
            {
                Success = true,
                Message = $"Đã tạo {inventories.Count} bản ghi tồn kho phòng.",
                RoomInventories = inventories
            };
        }
    }
}


