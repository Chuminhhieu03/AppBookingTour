using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.RoomInventories.DeleteRoomInventory;

public class DeleteRoomInventoryCommandHandler : IRequestHandler<DeleteRoomInventoryCommand, DeleteRoomInventoryResponse>
{
	private readonly IUnitOfWork _unitOfWork;

	public DeleteRoomInventoryCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<DeleteRoomInventoryResponse> Handle(DeleteRoomInventoryCommand request, CancellationToken cancellationToken)
	{
		var roomInventory = await _unitOfWork.Repository<RoomInventory>().GetByIdAsync(request.Id, cancellationToken);
		if (roomInventory == null)
		{
			throw new KeyNotFoundException("Không tìm thấy tồn kho phòng");
		}

		_unitOfWork.Repository<RoomInventory>().Remove(roomInventory);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return new DeleteRoomInventoryResponse
		{
			Success = true,
			Message = "Xóa thành công"
		};
	}
}

