using MediatR;

namespace MyMedia.Application.Commands.User
{
    public record RejectFollowRequestCommand(Guid ReceiverId, Guid RequesterId) : IRequest;
}
