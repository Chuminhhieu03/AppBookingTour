using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Discounts.DeleteDiscount;

public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand, DeleteDiscountResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDiscountCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteDiscountCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteDiscountResponse> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = await _unitOfWork.Repository<Discount>().GetByIdAsync(request.Id, cancellationToken);

        if (discount == null)
        {
            throw new KeyNotFoundException("Không tìm thấy mã giảm giá");
        }

        // 1. Lấy toàn bộ ItemDiscount liên quan
        var itemDiscounts = await _unitOfWork.Repository<ItemDiscount>()
            .FindAsync(x => x.DiscountId == request.Id, cancellationToken);

        // 2. Xóa ItemDiscounts trước
        if (itemDiscounts.Any())
        {
            _unitOfWork.Repository<ItemDiscount>().RemoveRange(itemDiscounts);
        }

        // 3. Xóa Discount
        _unitOfWork.Repository<Discount>().Remove(discount);

        // 4. Commit
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DeleteDiscountResponse
        {
            Success = true,
            Message = "Xóa thành công"
        };

    }
}
