using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.DeleteAccommodation;

public class DeleteAccommodationCommandHandler : IRequestHandler<DeleteAccommodationCommand, DeleteAccommodationResponse>
{
	private readonly IUnitOfWork _unitOfWork;

	public DeleteAccommodationCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<DeleteAccommodationResponse> Handle(DeleteAccommodationCommand request, CancellationToken cancellationToken)
	{
		var accommodation = await _unitOfWork.Repository<Accommodation>().GetByIdAsync(request.Id, cancellationToken);
		if (accommodation == null)
		{
			throw new KeyNotFoundException("Không tìm thấy chỗ ở");
		}

		_unitOfWork.Repository<Accommodation>().Remove(accommodation);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return new DeleteAccommodationResponse
		{
			Success = true,
			Message = "Xóa thành công"
		};
	}
}


