using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Bookings.InitiatePayment;

public class InitiatePaymentCommandHandler : IRequestHandler<InitiatePaymentCommand, InitiatePaymentResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVNPayService _vnPayService;
    private readonly IQRCodeService _qrCodeService;
    private readonly ILogger<InitiatePaymentCommandHandler> _logger;

    public InitiatePaymentCommandHandler(
        IUnitOfWork unitOfWork,
        IVNPayService vnPayService,
        IQRCodeService qrCodeService,
        ILogger<InitiatePaymentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _vnPayService = vnPayService;
        _qrCodeService = qrCodeService;
        _logger = logger;
    }

    public async Task<InitiatePaymentResponseDTO> Handle(InitiatePaymentCommand request, CancellationToken cancellationToken)
    {
        var req = request.Request;
        
        _logger.LogInformation("Initiating payment for booking {BookingId}", req.BookingId);

        // 1. Get booking details
        var booking = await _unitOfWork.Bookings.GetBookingWithDetailsAsync(req.BookingId, cancellationToken);
        if (booking == null)
        {
            return new InitiatePaymentResponseDTO
            {
                Success = false,
                Message = "Booking không tồn tại",
                Amount = 0
            };
        }

        // 2. Validate booking status
        if (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Confirmed)
        {
            return new InitiatePaymentResponseDTO
            {
                Success = false,
                Message = "Booking không ở trạng thái để thanh toán",
                Amount = booking.FinalAmount
            };
        }

        // 3. Check payment deadline
        var paymentDeadline = booking.CreatedAt.AddMinutes(15);
        if (DateTime.UtcNow > paymentDeadline && booking.Status == BookingStatus.Pending)
        {
            booking.Status = BookingStatus.Cancelled;
            _unitOfWork.Bookings.Update(booking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new InitiatePaymentResponseDTO
            {
                Success = false,
                Message = "Booking đã hết hạn thanh toán",
                Amount = booking.FinalAmount
            };
        }

        // 4. Get payment method
        var paymentMethod = await _unitOfWork.PaymentMethods.GetByIdAsync(req.PaymentMethodId, cancellationToken);
        if (paymentMethod == null || !paymentMethod.IsActive)
        {
            return new InitiatePaymentResponseDTO
            {
                Success = false,
                Message = "Phương thức thanh toán không hợp lệ",
                Amount = booking.FinalAmount
            };
        }

        // 5. Calculate payment amount
        var paidAmount = booking.Payments
            .Where(p => p.Status == PaymentStatus.Completed)
            .Sum(p => p.Amount);

        var remainingAmount = booking.FinalAmount - paidAmount;

        if (remainingAmount <= 0)
        {
            return new InitiatePaymentResponseDTO
            {
                Success = false,
                Message = "Booking ?ã ???c thanh toán ??y ??",
                Amount = booking.FinalAmount
            };
        }

        //var paymentAmount = booking.PaymentType == PaymentType.Deposit 
        //    ? booking.FinalAmount * 0.3m  // 30% deposit
        //    : remainingAmount;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // 6. Create payment record
            var paymentNumber = GeneratePaymentNumber();
            var payment = new Payment
            {
                BookingId = booking.Id,
                PaymentMethodId = paymentMethod.Id,
                PaymentNumber = paymentNumber,
                Amount = remainingAmount,
                PaymentDate = DateTime.UtcNow,
                Status = PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Payments.AddAsync(payment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 7. Generate payment URL based on payment method
            string? paymentUrl = null;
            string? qrCodeBase64 = null;

            if (paymentMethod.Code == "VNPAY")
            {
                var orderInfo = $"Thanh toan booking {booking.BookingCode}";
                paymentUrl = _vnPayService.CreatePaymentUrl(
                    booking.Id, 
                    remainingAmount, 
                    orderInfo, 
                    req.IpAddress);

                // TODO: Đưa vào config
                paymentUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html" + paymentUrl;
                // Generate QR Code
                var qrCodeBytes = _qrCodeService.GeneratePaymentQRCode(paymentUrl);
                qrCodeBase64 = Convert.ToBase64String(qrCodeBytes);

                // Store payment URL in notes
                payment.Notes = paymentUrl;
                _unitOfWork.Payments.Update(payment);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            _logger.LogInformation("Payment initiated successfully. Payment Number: {PaymentNumber}", paymentNumber);

            return new InitiatePaymentResponseDTO
            {
                Success = true,
                Message = "Khởi tạo thanh toán thành công",
                PaymentUrl = paymentUrl,
                QRCodeBase64 = qrCodeBase64,
                PaymentNumber = paymentNumber,
                PaymentExpiry = DateTime.UtcNow.AddMinutes(15),
                Amount = remainingAmount
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogError(ex, "Error initiating payment for booking {BookingId}", req.BookingId);
            throw;
        }
    }

    private string GeneratePaymentNumber()
    {
        return $"PAY{DateTime.UtcNow:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
    }
}
