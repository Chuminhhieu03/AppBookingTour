using AppBookingTour.Application.Features.Auth.Login;
using AppBookingTour.Application.Features.Auth.Register;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppBookingTour.Infrastructure.Services
{
    public class AuthService(
        UserManager<User> _userManager,
        ILogger<AuthService> _logger,
        IJwtService _jwtService
        ) : IAuthService
    {
        // Method dùng để đăng ký tài khoản 
        public async Task<RegisterCommandResponse> RegisterAsync(RegisterCommand request)
        {
            // Ghi log 
            _logger.LogInformation("Bắt đầu quá trình tạo tài khoản cho email: {Email} và userName: {UserName}", request.Email, request.UserName);

            // Kiểm tra email của người dùng đã tồn tại chưa 
            var exitEmail = await _userManager.FindByEmailAsync(request.Email);
            if (exitEmail != null)
            {
                return new RegisterCommandResponse(false, "Email này đã tồn tại trong hệ thống, bạn vui lòng đổi sang email khác");
            }
            var exitUserName = await _userManager.FindByNameAsync(request.UserName);
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
            try
            {
                var resultCreateRole = await _userManager.AddToRoleAsync(newUser, request.UserType.ToString());
                if (!resultCreateRole.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    return new RegisterCommandResponse(false, errors);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CHUI");
            }

            _logger.LogInformation("Đã tạo tài khoản thành công cho {Email}", request.Email);

            return new RegisterCommandResponse(true, "Tạo tài khoản thành công");
        }
        // Method dùng để tạo token generate Email Confirm
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
        // Method dùng để xóa tài khoản 
        public async Task DeleteAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        // Method dùng để đăng nhập tài khoản 
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

            // Kiểm tra user đã confirm mail chưa 
            if(!user.EmailConfirmed)
            {
                return new LoginCommandResponse
                {
                    Success = false,
                    Message = "Bạn cần xác nhận email trước khi dùng tiếp hệ thống"
                };
            };

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
            // Tạo access token 
            var accessToken = _jwtService.GenerateAccessToken(user, roles, out var accessTokenExpiry);

            // Tạo refresh token
            var refreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(_jwtService.GetRefreshTokenExpiryDays());

            // Cập nhật refresh token cho user
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiry;

            _logger.LogInformation("Hệ thống xác thực đăng nhập thành công cho {Email}", user.Email);

            return new LoginCommandResponse
            {
                Success = true,
                Token = accessToken,
                Expiration = accessTokenExpiry,
                Message = "Đăng nhập thành công, bạn sẽ được chuyển hướng sang trang chính",
                RefreshToken = refreshToken,
                RefreshTokenExpiry = refreshTokenExpiry,
            };
        }


        public async Task<LoginCommandResponse> RefreshTokenAsync(string refreshToken)
        {
            // Tìm kiếm user dựa trên refresh token 
            var user = await _userManager.Users.FirstOrDefaultAsync(user => user.RefreshToken  == refreshToken);
            if(user == null)
            {
                return new LoginCommandResponse
                {
                    Success = false,
                    Message = "Refresh Token không hợp lệ, vui lòng đăng nhập lại"
                };
            }

            // Kiểm tra tiếp thời hạn của refresh token
            if (user.RefreshTokenExpiryTime.HasValue && user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new LoginCommandResponse
                {
                    Success = false,
                    Message = "Refresh Token đã hết hạn, vui lòng đăng nhập lại"
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            // Tạo access token 
            var accessToken = _jwtService.GenerateAccessToken(user, roles, out var accessTokenExpiry);

            // Tạo refresh token
            var newRefreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(_jwtService.GetRefreshTokenExpiryDays());

            // Cập nhật refresh token cho user
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiry;

            _logger.LogInformation("Hệ thống đã cấp mã refresh token mới cho {Email}", user.Email);

            return new LoginCommandResponse
            {
                Success = true,
                Token = accessToken,
                Expiration = accessTokenExpiry,
                Message = "Refesh token thành công",
                RefreshToken = refreshToken,
                RefreshTokenExpiry = refreshTokenExpiry,
            };
        }

    }
}
               
