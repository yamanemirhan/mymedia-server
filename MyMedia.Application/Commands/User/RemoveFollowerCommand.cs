using MediatR;

namespace MyMedia.Application.Commands.User
{
    public record RemoveFollowerCommand(Guid CurrentUserId, Guid FollowerId) : IRequest<Unit>;
}
