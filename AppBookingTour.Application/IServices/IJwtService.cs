using System.Security.Claims;

namespace AppBookingTour.Application.IServices
{
    public interface IJwtService
    {
        //string GenerateToken(object userObj, IList<string> roles);
        ClaimsPrincipal? ValidateToken(string token);
        string GenerateRefreshToken();
    }
}
