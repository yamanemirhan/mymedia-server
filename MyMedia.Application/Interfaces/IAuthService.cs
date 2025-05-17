using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterUserAsync(string username, string email, string password);
        Task<AuthResponseDto> LoginUserAsync(string email, string password);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
    }
}
