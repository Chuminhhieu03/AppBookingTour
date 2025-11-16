
using AppBookingTour.Application.IServices;
using QRCoder;

namespace AppBookingTour.Infrastructure.Services;

public class QRCodeService : IQRCodeService
{
    public byte[] GenerateQRCode(string text, int width = 300, int height = 300)
    {
        using var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }

    public byte[] GeneratePaymentQRCode(string paymentUrl)
    {
        return GenerateQRCode(paymentUrl, 400, 400);
    }
}
