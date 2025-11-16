using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Bookings.GetPaymentStatus;

public class GetPaymentStatusQueryHandler : IRequestHandler<GetPaymentStatusQuery, PaymentStatusDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetPaymentStatusQueryHandler> _logger;

    public GetPaymentStatusQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetPaymentStatusQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaymentStatusDTO> Handle(GetPaymentStatusQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting payment status for booking {BookingId}", request.BookingId);

        var booking = await _unitOfWork.Bookings.GetBookingWithDetailsAsync(request.BookingId, cancellationToken);

        if (booking == null)
            throw new KeyNotFoundException($"Booking v?i ID {request.BookingId} không t?n t?i");

        var paidAmount = booking.Payments
            .Where(p => p.Status == PaymentStatus.Completed)
            .Sum(p => p.Amount);

        var remainingAmount = booking.FinalAmount - paidAmount;

        var paymentDeadline = booking.CreatedAt.AddMinutes(15);
        var isExpired = DateTime.UtcNow > paymentDeadline && booking.Status == BookingStatus.Pending;

        var paymentHistory = booking.Payments
            .OrderByDescending(p => p.PaymentDate)
            .Select(p => new PaymentHistoryDTO
            {
                Id = p.Id,
                PaymentNumber = p.PaymentNumber,
                Amount = p.Amount,
                PaymentMethodName = p.PaymentMethod?.Name ?? "N/A",
                Status = p.Status,
                PaymentDate = p.PaymentDate,
                TransactionId = p.TransactionId
            })
            .ToList();

        var lastPayment = booking.Payments
            .Where(p => p.Status == PaymentStatus.Completed)
            .OrderByDescending(p => p.PaymentDate)
            .FirstOrDefault();

        return new PaymentStatusDTO
        {
            BookingId = booking.Id,
            BookingCode = booking.BookingCode,
            BookingStatus = booking.Status,
            TotalAmount = booking.FinalAmount,
            PaidAmount = paidAmount,
            RemainingAmount = remainingAmount,
            PaymentHistory = paymentHistory,
            LastPaymentDate = lastPayment?.PaymentDate,
            PaymentDeadline = paymentDeadline,
            IsExpired = isExpired
        };
    }
}
