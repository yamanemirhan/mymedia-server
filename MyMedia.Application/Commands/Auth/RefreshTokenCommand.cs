using MediatR;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Commands.Auth
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponseDto>;
}
