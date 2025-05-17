using MediatR;

namespace MyMedia.Application.Commands.User
{
    public record FollowUserCommand(Guid CurrentUserId, Guid TargetUserId) : IRequest;
}
