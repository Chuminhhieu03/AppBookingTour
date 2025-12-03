using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.RoomInventories.BulkDeleteRoomInventory
{
    public class BulkDeleteRoomInventoryHandler
        : IRequestHandler<BulkDeleteRoomInventoryCommand, BulkDeleteRoomInventoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BulkDeleteRoomInventoryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BulkDeleteRoomInventoryResponse> Handle(
            BulkDeleteRoomInventoryCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Request is null || request.Request.Ids.Count == 0)
            {
                return new BulkDeleteRoomInventoryResponse
                {
                    Success = false,
                    Message = "Danh sách id cần xóa không được để trống."
                };
            }

            var ids = request.Request.Ids.Distinct().ToArray();

            var inventories = (await _unitOfWork.RoomInventories
                .FindAsync(x => ids.Contains(x.Id), cancellationToken))
                .ToList();

            if (inventories.Count == 0)
            {
                return new BulkDeleteRoomInventoryResponse
                {
                    Success = false,
                    Message = "Không tìm thấy room inventory để xóa."
                };
            }

            _unitOfWork.RoomInventories.RemoveRange(inventories);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new BulkDeleteRoomInventoryResponse
            {
                Success = true,
                Message = $"Đã xóa {inventories.Count} room inventory.",
                DeletedCount = inventories.Count,
                DeletedInventories = inventories
            };
        }
    }
}


