using AppBookingTour.Application.Features.Auth.Login;
using AppBookingTour.Application.Features.Auth.Register;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppBookingTour.Infrastructure.Services
{
    public class AuthService(
        UserManager<User> _userManager,
        ILogger<AuthService> _logger,
        IOptions<JwtSettings> jwtSetting
        ) : IAuthService
    {
        private readonly JwtSettings _jwtSettings = jwtSetting.Value;
        public async Task<RegisterCommandResponse> RegisterAsync(RegisterCommand request)
        {
            // Ghi log 
            _logger.LogInformation("Bắt đầu quá trình tạo tài khoản cho email: {Email} và userName: {UserName}", request.Email, request.UserName);

            // Kiểm tra email của người dùng đã tồn tại chưa 
            var exitEmail = _userManager.FindByEmailAsync( request.Email );
            if (exitEmail != null)
            {
                return new RegisterCommandResponse(false, "Email này đã tồn tại trong hệ thống, bạn vui lòng đổi sang email khác");
            }
            var exitUserName = _userManager.FindByNameAsync( request.UserName );
            if (exitUserName != null)
            {
                return new RegisterCommandResponse(false, "Tài khoản này đã tồn tài trong hệ thống, bạn vui lòng chọn tài khoản khác");
            }
            
            var newUser = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.Phone,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                Address = request.Address,
                UserType = request.UserType,
            };

            // Hàm built-in để tạo user mới trong hệ thống
            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return new RegisterCommandResponse(false, errors);
            }

            var resultCreateRole = await _userManager.AddToRoleAsync(newUser, request.UserType.ToString());
            if (!resultCreateRole.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return new RegisterCommandResponse(false, errors);
            }

            _logger.LogInformation("Đã tạo tài khoản thành công cho {Email}", request.Email);

            return new RegisterCommandResponse(true, "Tạo tài khoản thành công");
        }

        public async Task<string> GenerateTokenEmailConfirm(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogError("Không tìm thấy {Email} tồn tại trong hệ thống để tạo email email xác nhận", email);
                throw new InvalidOperationException("Không tìm thấy email trong hệ thống");
            }
            // Tạo mã token để gửi cho user bằng hàm built-in
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return token;
        }

        public async Task DeleteAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task<LoginCommandResponse> LoginAsync(LoginCommand request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new LoginCommandResponse
                {
                    Success = false,
                    Message = "Email đăng nhập không tồn tại trong hệ thống"
                };
            }
            var checkValidUser = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!checkValidUser)
            {
                return new LoginCommandResponse
                {
                    Success = false,
                    Message = "Mật khẩu bạn nhập không chính xác"
                };
            }

            // Lấy ra roles của user 
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            List<Claim> claims = [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
                // Đây là thêm role cho claims
                ..roles.Select(role => new Claim(ClaimTypes.Role, role)),
            ];

            // Tạo "bản thiết kế" cho JWT" để gửi cho client
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginCommandResponse
            {
                Success = true,
                Message = "Login successful",
                Token = tokenHandler.WriteToken(token),
                Expiration = tokenDescriptor.Expires
            };
        }
    }
}
