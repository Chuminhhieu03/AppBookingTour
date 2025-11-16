
using AppBookingTour.Application.Common.Settings;
using AppBookingTour.Application.IServices;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace AppBookingTour.Infrastructure.Services;

public class VNPayService : IVNPayService
{
    private readonly VNPaySettings _settings;

    public VNPayService(IOptions<VNPaySettings> settings)
    {
        _settings = settings.Value;
    }

    public string CreatePaymentUrl(int bookingId, decimal amount, string orderInfo, string ipAddress)
    {
        var vnpay = new VNPayLibrary();
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(_settings.TimeZoneId);
        var createDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

        vnpay.AddRequestData("vnp_Version", _settings.Version);
        vnpay.AddRequestData("vnp_Command", _settings.Command);
        vnpay.AddRequestData("vnp_TmnCode", _settings.TmnCode);
        vnpay.AddRequestData("vnp_Amount", ((long)(amount * 100)).ToString());
        vnpay.AddRequestData("vnp_CreateDate", createDate.ToString("yyyyMMddHHmmss"));
        vnpay.AddRequestData("vnp_CurrCode", _settings.CurrCode);
        vnpay.AddRequestData("vnp_IpAddr", ipAddress);
        vnpay.AddRequestData("vnp_Locale", _settings.Locale);
        vnpay.AddRequestData("vnp_OrderInfo", orderInfo);
        vnpay.AddRequestData("vnp_OrderType", "other");
        vnpay.AddRequestData("vnp_ReturnUrl", _settings.ReturnUrl);
        vnpay.AddRequestData("vnp_TxnRef", $"BK{bookingId:D8}_{createDate:yyyyMMddHHmmss}");
        
        var expireTime = createDate.AddMinutes(_settings.PaymentTimeout);
        vnpay.AddRequestData("vnp_ExpireDate", expireTime.ToString("yyyyMMddHHmmss"));

        var paymentUrl = vnpay.CreateRequestUrl(_settings.Url, _settings.HashSecret);
        return paymentUrl;
    }

    public bool ValidateSignature(Dictionary<string, string> vnpayData, string secureHash)
    {
        var vnpay = new VNPayLibrary();
        foreach (var kvp in vnpayData)
        {
            if (!string.IsNullOrEmpty(kvp.Value) && kvp.Key.StartsWith("vnp_"))
            {
                vnpay.AddResponseData(kvp.Key, kvp.Value);
            }
        }

        var checkSignature = vnpay.ValidateSignature(secureHash, _settings.HashSecret);
        return checkSignature;
    }

    public (bool Success, string Message, string TransactionId) ProcessPaymentCallback(Dictionary<string, string> vnpayData)
    {
        var responseCode = vnpayData.GetValueOrDefault("vnp_ResponseCode");
        var transactionId = vnpayData.GetValueOrDefault("vnp_TransactionNo", string.Empty);
        var bankCode = vnpayData.GetValueOrDefault("vnp_BankCode", string.Empty);

        if (responseCode == "00")
        {
            return (true, "Giao dịch thành công", transactionId);
        }

        var errorMessage = GetVNPayErrorMessage(responseCode ?? "99");
        return (false, errorMessage, transactionId);
    }

    private static string GetVNPayErrorMessage(string responseCode)
    {
        return responseCode switch
        {
            "07" => "Trừ tiền thành công. Giao dịch bị nghi ngờ (liên quan tới lừa đảo, giao dịch bất thường).",
            "09" => "Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng chưa đăng ký dịch vụ InternetBanking tại ngân hàng.",
            "10" => "Giao dịch không thành công do: Khách hàng xác thực thông tin thẻ/tài khoản không đúng quá 3 lần",
            "11" => "Giao dịch không thành công do: Đã hết hạn chờ thanh toán. Xin quý khách vui lòng thực hiện lại giao dịch.",
            "12" => "Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng bị khóa.",
            "13" => "Giao dịch không thành công do Quý khách nhập sai mật khẩu xác thực giao dịch (OTP).",
            "24" => "Giao dịch không thành công do: Khách hàng hủy giao dịch",
            "51" => "Giao dịch không thành công do: Tài khoản của quý khách không đủ số dư để thực hiện giao dịch.",
            "65" => "Giao dịch không thành công do: Tài khoản của Quý khách đã vượt quá hạn mức giao dịch trong ngày.",
            "75" => "Ngân hàng thanh toán đang bảo trì.",
            "79" => "Giao dịch không thành công do: KH nhập sai mật khẩu thanh toán quá số lần quy định.",
            _ => "Giao dịch không thành công."
        };
    }

    private class VNPayLibrary
    {
        private readonly SortedList<string, string> _requestData = new(new VNPayComparer());
        private readonly SortedList<string, string> _responseData = new(new VNPayComparer());

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string CreateRequestUrl(string baseUrl, string hashSecret)
        {
            var data = new StringBuilder();
            foreach (var kv in _requestData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }

            var queryString = data.ToString();
            if (queryString.Length > 0)
            {
                queryString = queryString.Remove(queryString.Length - 1, 1);
            }

            var signData = queryString;
            var vnpSecureHash = HmacSHA512(hashSecret, signData);
            queryString += "&vnp_SecureHash=" + vnpSecureHash;

            return baseUrl + "?" + queryString;
        }

        public bool ValidateSignature(string inputHash, string hashSecret)
        {
            var data = new StringBuilder();
            foreach (var kv in _responseData.Where(kv => kv.Key != "vnp_SecureHash" && kv.Key != "vnp_SecureHashType"))
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }

            var signData = data.ToString();
            if (signData.Length > 0)
            {
                signData = signData.Remove(signData.Length - 1, 1);
            }

            var myChecksum = HmacSHA512(hashSecret, signData);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private static string HmacSHA512(string key, string data)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var dataBytes = Encoding.UTF8.GetBytes(data);
            using var hmac = new HMACSHA512(keyBytes);
            var hashBytes = hmac.ComputeHash(dataBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        private class VNPayComparer : IComparer<string>
        {
            public int Compare(string? x, string? y)
            {
                if (x == y) return 0;
                if (x == null) return -1;
                if (y == null) return 1;
                var vnpCompare = CompareInfo.GetCompareInfo("en-US");
                return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
            }
        }
    }
}
