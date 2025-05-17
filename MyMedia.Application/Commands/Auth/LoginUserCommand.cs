using MediatR;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Commands.Auth
{
    public record LoginUserCommand(string Email, string Password) : IRequest<AuthResponseDto>;
}
