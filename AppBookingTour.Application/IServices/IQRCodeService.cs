
namespace AppBookingTour.Application.IServices;

public interface IQRCodeService
{
    /// <summary>
    /// Tạo QR Code từ text
    /// </summary>
    byte[] GenerateQRCode(string text, int width = 300, int height = 300);

    /// <summary>
    /// Tạo QR Code cho thanh toán VNPay
    /// </summary>
    byte[] GeneratePaymentQRCode(string paymentUrl);
}
