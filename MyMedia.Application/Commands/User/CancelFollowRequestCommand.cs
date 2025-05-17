using MediatR;

namespace MyMedia.Application.Commands.User
{
    public record CancelFollowRequestCommand(Guid RequesterId, Guid ReceiverId) : IRequest;
}
