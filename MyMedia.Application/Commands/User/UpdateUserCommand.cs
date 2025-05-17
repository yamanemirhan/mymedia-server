using MediatR;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Commands.User
{
    public record UpdateUserCommand(
        Guid UserId,
        string Username,
        string? AvatarUrl,
        string? Description,
        bool IsPrivate
    ) : IRequest<UserResponseDto>;
}
