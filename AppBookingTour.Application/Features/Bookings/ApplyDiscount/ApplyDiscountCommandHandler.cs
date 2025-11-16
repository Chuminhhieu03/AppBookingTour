using AppBookingTour.Application.IRepositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Bookings.ApplyDiscount;

public class ApplyDiscountCommandHandler : IRequestHandler<ApplyDiscountCommand, ApplyDiscountResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ApplyDiscountCommandHandler> _logger;

    public ApplyDiscountCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<ApplyDiscountCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApplyDiscountResponseDTO> Handle(ApplyDiscountCommand request, CancellationToken cancellationToken)
    {
        var req = request.Request;
        
        _logger.LogInformation("Applying discount code: {Code} for amount: {Amount}", req.DiscountCode, req.TotalAmount);

        var discount = await _unitOfWork.Discounts
            .FirstOrDefaultAsync(
                d => d.Code == req.DiscountCode && d.Status == 1,
                cancellationToken);

        if (discount == null)
        {
            return new ApplyDiscountResponseDTO
            {
                IsValid = false,
                Message = "Mã gi?m giá không t?n t?i ho?c ?ã h?t hi?u l?c",
                DiscountAmount = 0,
                FinalAmount = req.TotalAmount
            };
        }

        // Validate discount
        var now = DateTime.UtcNow;
        
        if (discount.StartEffectedDtg.HasValue && now < discount.StartEffectedDtg.Value)
        {
            return new ApplyDiscountResponseDTO
            {
                IsValid = false,
                Message = $"Mã gi?m giá ch? có hi?u l?c t? {discount.StartEffectedDtg.Value:dd/MM/yyyy}",
                DiscountAmount = 0,
                FinalAmount = req.TotalAmount
            };
        }

        if (discount.EndEffectedDtg.HasValue && now > discount.EndEffectedDtg.Value)
        {
            return new ApplyDiscountResponseDTO
            {
                IsValid = false,
                Message = "Mã gi?m giá ?ã h?t h?n s? d?ng",
                DiscountAmount = 0,
                FinalAmount = req.TotalAmount
            };
        }

        if (discount.RemainQuantity.HasValue && discount.RemainQuantity.Value <= 0)
        {
            return new ApplyDiscountResponseDTO
            {
                IsValid = false,
                Message = "Mã gi?m giá ?ã h?t l??t s? d?ng",
                DiscountAmount = 0,
                FinalAmount = req.TotalAmount
            };
        }

        if (discount.MinimumOrderAmount.HasValue && req.TotalAmount < discount.MinimumOrderAmount.Value)
        {
            return new ApplyDiscountResponseDTO
            {
                IsValid = false,
                Message = $"??n hàng t?i thi?u {discount.MinimumOrderAmount.Value:N0} VN? ?? áp d?ng mã này",
                DiscountAmount = 0,
                FinalAmount = req.TotalAmount
            };
        }

        // Check if user has already used this discount
        if (req.UserId.HasValue)
        {
            var hasUsed = await _unitOfWork.DiscountUsages
                .ExistsAsync(
                    du => du.DiscountId == discount.Id && du.UserId == req.UserId.Value,
                    cancellationToken);

            if (hasUsed)
            {
                return new ApplyDiscountResponseDTO
                {
                    IsValid = false,
                    Message = "B?n ?ã s? d?ng mã gi?m giá này r?i",
                    DiscountAmount = 0,
                    FinalAmount = req.TotalAmount
                };
            }
        }

        // Calculate discount
        var discountAmount = CalculateDiscount(discount, req.TotalAmount);
        var finalAmount = req.TotalAmount - discountAmount;

        _logger.LogInformation("Discount applied successfully. Original: {Original}, Discount: {Discount}, Final: {Final}",
            req.TotalAmount, discountAmount, finalAmount);

        return new ApplyDiscountResponseDTO
        {
            IsValid = true,
            Message = "Áp d?ng mã gi?m giá thành công",
            DiscountCode = discount.Code,
            DiscountName = discount.Name,
            DiscountAmount = discountAmount,
            FinalAmount = finalAmount
        };
    }

    private decimal CalculateDiscount(Domain.Entities.Discount discount, decimal totalAmount)
    {
        if (!discount.DiscountPercent.HasValue)
            return 0;

        var discountAmount = totalAmount * (discount.DiscountPercent.Value / 100);

        if (discount.MaximumDiscount.HasValue && discountAmount > discount.MaximumDiscount.Value)
        {
            discountAmount = discount.MaximumDiscount.Value;
        }

        return Math.Round(discountAmount, 0);
    }
}
