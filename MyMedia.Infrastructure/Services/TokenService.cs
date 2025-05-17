using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using MyMedia.Application.Interfaces;
using MyMedia.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using MyMedia.Shared;
using System.Security.Cryptography;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtSettings _jwtSettings;

        public TokenService(IHttpContextAccessor httpContextAccessor, IOptions<JwtSettings> jwtOptions)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtSettings = jwtOptions.Value;
        }

        public void SetAccessToken(string token, DateTime expiry)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("accessToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expiry
            });
        }

        public void SetRefreshToken(string token, DateTime expiry)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expiry
            });
        }

        public string GetAccessToken()
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"] ?? string.Empty;
        }

        public string GetRefreshToken()
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"] ?? string.Empty;
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
        public AuthResponseDto GenerateAuthTokens(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var accessToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessTokenJwt = tokenHandler.WriteToken(accessToken);

            return new AuthResponseDto
            {
                AccessToken = accessTokenJwt,
                AccessTokenExpireAt = DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                RefreshToken = user.RefreshToken,
                RefreshTokenExpireAt = user.RefreshTokenExpireAt!.Value,
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                IsAdmin = user.IsAdmin
            };
        }

        public void ClearTokens()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("accessToken");
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");
        }
    }
}
