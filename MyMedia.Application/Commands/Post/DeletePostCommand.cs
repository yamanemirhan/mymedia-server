using MediatR;

namespace MyMedia.Application.Commands.Post
{
    public record DeletePostCommand(Guid PostId, Guid UserId) : IRequest<Unit>;
}
