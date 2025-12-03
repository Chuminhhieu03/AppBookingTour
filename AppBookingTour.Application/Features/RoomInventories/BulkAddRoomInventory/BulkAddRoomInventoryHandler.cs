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

            var roomTypeId = payload.RoomTypeId;

            // 🔥 Lấy tất cả inventory trong range để tránh query từng ngày
            var existingInventories = await _unitOfWork.RoomInventories
                .GetByRoomTypeAndDateRange(roomTypeId, fromDate, toDate.AddDays(1));

            var inventoriesToAdd = new List<RoomInventory>();
            var inventoriesToUpdate = new List<RoomInventory>();

            for (var current = fromDate; current <= toDate; current = current.AddDays(1))
            {
                var existing = existingInventories
                    .FirstOrDefault(x => x.Date.Date == current.Date);

                if (existing != null)
                {
                    // 🔄 Update
                    existing.BasePrice = payload.BasePrice;
                    existing.BasePriceAdult = payload.BasePriceAdult ?? payload.BasePrice;
                    existing.BasePriceChildren = payload.BasePriceChildren ?? payload.BasePrice;
                    existing.BookedRooms = payload.BookedRooms;

                    inventoriesToUpdate.Add(existing);
                }
                else
                {
                    // ➕ Add new
                    inventoriesToAdd.Add(new RoomInventory
                    {
                        RoomTypeId = payload.RoomTypeId,
                        Date = current,
                        BasePrice = payload.BasePrice,
                        BasePriceAdult = payload.BasePriceAdult ?? payload.BasePrice,
                        BasePriceChildren = payload.BasePriceChildren ?? payload.BasePrice,
                        BookedRooms = payload.BookedRooms
                    });
                }
            }

            // ➕ Thêm mới
            if (inventoriesToAdd.Any())
                await _unitOfWork.RoomInventories.AddRangeAsync(inventoriesToAdd, cancellationToken);

            // 🔄 EF tracking nên không cần gọi update explicit, nhưng nếu bạn có repository riêng thì gọi:
            if (inventoriesToUpdate.Any())
                _unitOfWork.RoomInventories.UpdateRange(inventoriesToUpdate);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new BulkAddRoomInventoryResponse
            {
                Success = true,
                Message = $"Đã cập nhật {inventoriesToUpdate.Count} bản ghi, tạo mới {inventoriesToAdd.Count} bản ghi.",
                RoomInventories = inventoriesToAdd.Concat(inventoriesToUpdate).ToList()
            };
        }

    }
}


