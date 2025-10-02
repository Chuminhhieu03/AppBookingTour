using AppBookingTour.Application.Features.Auth.Login;
using AppBookingTour.Application.Features.Auth.Register;

namespace AppBookingTour.Application.IServices
{
    public interface IAuthService
    {
        Task<RegisterCommandResponse> RegisterAsync(RegisterCommand request);
        Task<string> GenerateTokenEmailConfirm(string email);
        Task DeleteAsync(string userEmail);
        Task<LoginCommandResponse> LoginAsync(LoginCommand request);
    }
}
