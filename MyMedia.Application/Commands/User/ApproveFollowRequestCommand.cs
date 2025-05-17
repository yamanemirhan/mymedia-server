using MediatR;

namespace MyMedia.Application.Commands.User
{
    public record ApproveFollowRequestCommand(Guid CurrentUserId, Guid RequesterId) : IRequest;
}
