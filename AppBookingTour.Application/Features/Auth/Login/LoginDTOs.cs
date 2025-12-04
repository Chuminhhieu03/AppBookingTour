namespace AppBookingTour.Application.Features.Auth.Login
{
    public class LoginCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public string? Token { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime? Expiration { get; set; }
        // Trường refresh_token chỉ sử dụng để gửi token qua HTTP secure
        public string? RefreshToken { get; set; }
        public DateTimeOffset? RefreshTokenExpiry { get; set; }
    }
}
