using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AppBookingTour.Application.Features.Bookings.PaymentCallback;

public class PaymentCallbackCommandHandler : IRequestHandler<PaymentCallbackCommand, PaymentCallbackResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVNPayService _vnPayService;
    private readonly ILogger<PaymentCallbackCommandHandler> _logger;
    private readonly IConfiguration _configuration;

    public PaymentCallbackCommandHandler(
        IUnitOfWork unitOfWork,
        IVNPayService vnPayService,
        ILogger<PaymentCallbackCommandHandler> logger,
        IConfiguration configuration
        )
    {
        _unitOfWork = unitOfWork;
        _vnPayService = vnPayService;
        _logger = logger;
        _configuration = configuration; 
    }

    public async Task<PaymentCallbackResponseDTO> Handle(PaymentCallbackCommand request, CancellationToken cancellationToken)
    {
        var vnpayData = request.Request.VnPayData;
        
        _logger.LogInformation("Processing VNPay callback");

        // 1. Validate signature
        var secureHash = vnpayData.GetValueOrDefault("vnp_SecureHash", string.Empty);
        var isValidSignature = _vnPayService.ValidateSignature(vnpayData, secureHash);

        if (!isValidSignature)
        {
            _logger.LogWarning("Invalid VNPay signature");
            return new PaymentCallbackResponseDTO
            {
                Success = false,
                Message = "Chữ ký không hợp lệ"
            };
        }

        // 2. Process payment result
        var (success, message, transactionId) = _vnPayService.ProcessPaymentCallback(vnpayData);

        // 3. Extract booking info from vnp_TxnRef
        var txnRef = vnpayData.GetValueOrDefault("vnp_TxnRef", string.Empty);
        var amount = decimal.Parse(vnpayData.GetValueOrDefault("vnp_Amount", "0")) / 100;
        
        // TxnRef format: BK{BookingId:D8}_{Timestamp}
        var bookingIdStr = txnRef.Split('_')[0].Replace("BK", "").TrimStart('0');
        if (!int.TryParse(bookingIdStr, out var bookingId))
        {
            _logger.LogError("Invalid booking ID in TxnRef: {TxnRef}", txnRef);
            return new PaymentCallbackResponseDTO
            {
                Success = false,
                Message = "Mã giao d?ch không h?p l?"
            };
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // 4. Get booking and pending payment
            var booking = await _unitOfWork.Bookings.GetBookingWithDetailsAsync(bookingId, cancellationToken);
            if (booking == null)
            {
                _logger.LogError("Booking not found: {BookingId}", bookingId);
                return new PaymentCallbackResponseDTO
                {
                    Success = false,
                    Message = "Booking không tồn tại "
                };
            }

            var pendingPayment = booking.Payments
                .Where(p => p.Status == PaymentStatus.Pending)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefault();

            if (pendingPayment == null)
            {
                _logger.LogWarning("No pending payment found for booking {BookingId}", bookingId);
                return new PaymentCallbackResponseDTO
                {
                    Success = false,
                    Message = "Không tìm thấy giao d?ch thanh toán"
                };
            }

            // 5. Update payment status
            if (success)
            {
                pendingPayment.Status = PaymentStatus.Completed;
                pendingPayment.TransactionId = transactionId;
                pendingPayment.PaymentDate = DateTime.UtcNow;
                pendingPayment.GatewayResponse = JsonSerializer.Serialize(vnpayData);
                pendingPayment.UpdatedAt = DateTime.UtcNow;

                // 6. Update booking status
                var totalPaid = booking.Payments
                    .Where(p => p.Status == PaymentStatus.Completed)
                    .Sum(p => p.Amount) + pendingPayment.Amount;

                if (totalPaid >= booking.FinalAmount)
                {
                    booking.Status = BookingStatus.Paid;
                }
                else if (booking.PaymentType == PaymentType.Deposit && totalPaid > 0)
                {
                    booking.Status = BookingStatus.Confirmed;
                }

                booking.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.Payments.Update(pendingPayment);
                _unitOfWork.Bookings.Update(booking);

                _logger.LogInformation("Payment completed successfully for booking {BookingCode}", booking.BookingCode);
            }
            else
            {
                pendingPayment.Status = PaymentStatus.Failed;
                pendingPayment.TransactionId = transactionId;
                pendingPayment.GatewayResponse = JsonSerializer.Serialize(vnpayData);
                pendingPayment.Notes = message;
                pendingPayment.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.Payments.Update(pendingPayment);

                _logger.LogWarning("Payment failed for booking {BookingCode}: {Message}", booking.BookingCode, message);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var redirectFrontendUrl = success
                ? $"{GetFrontendUrl()}/booking/booking-success?bookingCode={booking.BookingCode}&transactionId={transactionId}"
                : $"{GetFrontendUrl()}/booking/booking-failed?message={Uri.EscapeDataString(message)}"; 

            return new PaymentCallbackResponseDTO
            {
                Success = success,
                Message = message,
                BookingCode = booking.BookingCode,
                BookingId = booking.Id,
                TransactionId = transactionId,
                RedirectUrl = redirectFrontendUrl,
                Amount = amount
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogError(ex, "Error processing payment callback");
            throw;
        }
    }

    private string GetFrontendUrl()
    {
        var frontendUrl = _configuration.GetSection("FE_Domain").Value;
        return frontendUrl ?? "http://localhost:3000";      
    }
}
