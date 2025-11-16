
namespace AppBookingTour.Application.IServices;

public interface IVNPayService
{
    /// <summary>
    /// Tạo URL thanh toán VNPay
    /// </summary>
    string CreatePaymentUrl(int bookingId, decimal amount, string orderInfo, string ipAddress);

    /// <summary>
    /// Xác thực callback từ VNPay
    /// </summary>
    bool ValidateSignature(Dictionary<string, string> vnpayData, string secureHash);

    /// <summary>
    /// Xử lý kết quả thanh toán từ VNPay
    /// </summary>
    (bool Success, string Message, string TransactionId) ProcessPaymentCallback(Dictionary<string, string> vnpayData);
}
