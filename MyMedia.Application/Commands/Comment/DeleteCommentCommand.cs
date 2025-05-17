using MediatR;

namespace MyMedia.Application.Commands.Comment
{
    public record DeleteCommentCommand(Guid CommentId, Guid UserId) : IRequest<Unit>;
}
