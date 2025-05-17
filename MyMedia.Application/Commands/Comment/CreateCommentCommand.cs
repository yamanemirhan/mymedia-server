using MediatR;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Commands.Comment
{
    public record CreateCommentCommand(
        Guid UserId,
        string Text,
        Guid PostId,
        Guid? ParentCommentId
    ) : IRequest<CommentResponseDto>;
}
