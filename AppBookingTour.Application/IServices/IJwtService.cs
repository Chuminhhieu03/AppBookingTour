using AppBookingTour.Domain.Entities;
using System.Security.Claims;

namespace AppBookingTour.Application.IServices
{
    public interface IJwtService
    {
        // Generate access token for a user with roles. Returns JWT and outputs expiry time (UTC)
        string GenerateAccessToken(User user, IList<string> roles, out DateTime expiresAtUtc);
        ClaimsPrincipal? ValidateToken(string token);
        string GenerateRefreshToken();
        int GetRefreshTokenExpiryDays();
    }
}
