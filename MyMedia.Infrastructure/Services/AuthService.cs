using MyMedia.Application.Interfaces;
using MyMedia.Domain.Entities;
using MyMedia.Infrastructure.Data;
using MyMedia.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using MyMedia.Shared.GlobalErrorHandling;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly ITokenService _tokenService;

        public AuthService(AppDbContext context, IOptions<JwtSettings> jwtOptions, IHttpContextAccessor httpContextAccessor, ITokenService tokenService)
        {
            _context = context;
            _jwtSettings = jwtOptions.Value;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> RegisterUserAsync(string username, string email, string password)
        {
            if (await IsEmailOrUsernameTakenAsync(email, username))
                throw new CustomException("Email or username already in use.", 400);

            var hashedPassword = HashPassword(password);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                PasswordHash = hashedPassword,
                RefreshToken = refreshToken,
                RefreshTokenExpireAt = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                CreatedAt = DateTime.Now,
                Avatar = new UserAvatarMedia
                {
                    AvatarUrl = "https://bu4gkqk43wetzryg.public.blob.vercel-storage.com/def-avatar-S4NnHLL63VypWynP8rLED2sUCNNoBi.jpg"
                }
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var responseDto = _tokenService.GenerateAuthTokens(user);

            _tokenService.SetAccessToken(responseDto.AccessToken, responseDto.AccessTokenExpireAt);
            _tokenService.SetRefreshToken(responseDto.RefreshToken, responseDto.RefreshTokenExpireAt);

            return responseDto;
        }

        public async Task<AuthResponseDto> LoginUserAsync(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || HashPassword(password) != user.PasswordHash)
                throw new CustomException("Invalid email or password.", 401);

            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpireAt = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationDays);
            user.UpdatedAt = DateTime.Now;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var responseDto = _tokenService.GenerateAuthTokens(user);

            _tokenService.SetAccessToken(responseDto.AccessToken, responseDto.AccessTokenExpireAt);
            _tokenService.SetRefreshToken(responseDto.RefreshToken, responseDto.RefreshTokenExpireAt);

            return responseDto;
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new CustomException("No refresh token provided.", 401);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null || user.RefreshTokenExpireAt < DateTime.Now)
                throw new CustomException("Invalid or expired refresh token.", 401);

            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpireAt = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationDays);
            user.UpdatedAt = DateTime.Now;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var responseDto = _tokenService.GenerateAuthTokens(user);

            _tokenService.SetAccessToken(responseDto.AccessToken, responseDto.AccessTokenExpireAt);
            _tokenService.SetRefreshToken(responseDto.RefreshToken, responseDto.RefreshTokenExpireAt);

            return responseDto;
        }

        private async Task<bool> IsEmailOrUsernameTakenAsync(string email, string username)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email || u.Username == username);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
