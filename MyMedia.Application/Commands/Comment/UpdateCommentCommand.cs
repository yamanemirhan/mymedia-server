using MediatR;

namespace MyMedia.Application.Commands.Comment
{
    public record UpdateCommentCommand(Guid CommentId, Guid UserId, string Text) : IRequest<Unit>;
}
