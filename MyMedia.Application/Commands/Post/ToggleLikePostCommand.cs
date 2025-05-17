using MediatR;

namespace MyMedia.Application.Commands.Post
{
    public record ToggleLikePostCommand(Guid PostId, Guid UserId) : IRequest<bool>;
}
