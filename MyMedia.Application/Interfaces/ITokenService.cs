using MyMedia.Application.Dtos.Response;
using MyMedia.Domain.Entities;

namespace MyMedia.Application.Interfaces
{
    public interface ITokenService
    {
        void SetAccessToken(string token, DateTime expiry);
        void SetRefreshToken(string token, DateTime expiry);
        string GetAccessToken();
        string GetRefreshToken();
        string GenerateRefreshToken();
        AuthResponseDto GenerateAuthTokens(User user);
        void ClearTokens();
    }
}
