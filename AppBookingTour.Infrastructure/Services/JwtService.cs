using AppBookingTour.Application.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppBookingTour.Infrastructure.Services;

/// <summary>
/// JWT service implementation
/// Implements IJwtService interface from Application layer
/// </summary>
public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _tokenExpirationMinutes;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = _configuration["JWT:SecretKey"] ?? throw new ArgumentNullException("JWT:SecretKey not configured");
        _issuer = _configuration["JWT:Issuer"] ?? throw new ArgumentNullException("JWT:Issuer not configured");
        _audience = _configuration["JWT:Audience"] ?? throw new ArgumentNullException("JWT:Audience not configured");
        _tokenExpirationMinutes = int.Parse(_configuration["JWT:TokenExpirationMinutes"] ?? "60");
    }

    //public string GenerateToken(object userObj, IList<string> roles)
    //{
    //    if (userObj is not ApplicationUser user)
    //        throw new ArgumentException("User must be ApplicationUser", nameof(userObj));

    //    var tokenHandler = new JwtSecurityTokenHandler();
    //    var key = Encoding.ASCII.GetBytes(_secretKey);

    //    var claims = new List<Claim>
    //    {
    //        new(ClaimTypes.NameIdentifier, user.Id),
    //        new(ClaimTypes.Email, user.Email!),
    //        new(ClaimTypes.Name, user.UserName!),
    //        new("FullName", user.FullName),
    //        new("UserType", user.UserType.ToString()),
    //        new("Status", user.Status.ToString()),
    //        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //        new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
    //    };

    //    // Add roles as claims
    //    foreach (var role in roles)
    //    {
    //        claims.Add(new Claim(ClaimTypes.Role, role));
    //    }

    //    var tokenDescriptor = new SecurityTokenDescriptor
    //    {
    //        Subject = new ClaimsIdentity(claims),
    //        Expires = DateTime.UtcNow.AddMinutes(_tokenExpirationMinutes),
    //        Issuer = _issuer,
    //        Audience = _audience,
    //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    //    };

    //    var token = tokenHandler.CreateToken(tokenDescriptor);
    //    return tokenHandler.WriteToken(token);
    //}

    public ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            
            if (validatedToken is not JwtSecurityToken jwtToken || 
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}