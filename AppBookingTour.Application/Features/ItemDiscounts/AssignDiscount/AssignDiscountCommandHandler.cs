using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.ItemDiscounts.AssignDiscount;

public class AssignDiscountCommandHandler : IRequestHandler<AssignDiscountCommand, AssignDiscountResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public AssignDiscountCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AssignDiscountResponse> Handle(AssignDiscountCommand request, CancellationToken cancellationToken)
    {
        var req = request.Request;
        // 1. Xóa các ItemDiscount hiện có của itemId và itemType
        var existingItemDiscounts = await _unitOfWork.Repository<ItemDiscount>()
            .FindAsync(x => x.ItemId == req.ItemId && x.ItemType == req.ItemType, cancellationToken);
        // 2. Thêm mới các ItemDiscount theo listDiscountId
        var newItemDiscounts = req.ListDiscountId
            .Select(discountId => new ItemDiscount
            {
                ItemId = req.ItemId,
                ItemType = req.ItemType,
                DiscountId = discountId
            })
            .ToList();
        await _unitOfWork.BeginTransactionAsync();
        if (existingItemDiscounts.Any())
        {
            _unitOfWork.Repository<ItemDiscount>().RemoveRange(existingItemDiscounts);
        }
        if (newItemDiscounts.Any())
        {
            await _unitOfWork.Repository<ItemDiscount>().AddRangeAsync(newItemDiscounts, cancellationToken);
        }

        // 3. Lưu thay đổi
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return new AssignDiscountResponse
        {
            Success = true,
            Message = "Gán discount thành công"
        };
    }
}

