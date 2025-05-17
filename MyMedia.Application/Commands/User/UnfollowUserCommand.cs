using MediatR;

namespace MyMedia.Application.Commands.User
{
    public record UnfollowUserCommand(Guid CurrentUserId, Guid TargetUserId) : IRequest;
}
