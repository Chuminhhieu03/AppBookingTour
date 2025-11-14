using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.RoomTypes.DeleteRoomType;

public class DeleteRoomTypeCommandHandler : IRequestHandler<DeleteRoomTypeCommand, DeleteRoomTypeResponse>
{
	private readonly IUnitOfWork _unitOfWork;

	public DeleteRoomTypeCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<DeleteRoomTypeResponse> Handle(DeleteRoomTypeCommand request, CancellationToken cancellationToken)
	{
		var roomType = await _unitOfWork.Repository<RoomType>().GetByIdAsync(request.Id, cancellationToken);
		if (roomType == null)
		{
			throw new KeyNotFoundException("Không tìm thấy loại phòng");
		}

		_unitOfWork.Repository<RoomType>().Remove(roomType);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return new DeleteRoomTypeResponse
		{
			Success = true,
			Message = "Xóa thành công"
		};
	}
}

