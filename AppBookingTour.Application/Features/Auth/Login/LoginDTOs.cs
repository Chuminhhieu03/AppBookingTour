namespace AppBookingTour.Application.Features.Auth.Login
{
    public class LoginCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
