using MediatR;

namespace MyMedia.Application.Commands.Comment
{
    public record ToggleLikeCommentCommand(Guid CommentId, Guid UserId) : IRequest<Unit>;
}
