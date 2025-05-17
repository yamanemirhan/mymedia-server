using MediatR;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Commands.Auth
{
    public record RegisterUserCommand(string Username, string Email, string Password) : IRequest<AuthResponseDto>;
}
