using MediatR;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Queries.Comment
{
    public record GetCommentsByPostIdQuery(Guid PostId) : IRequest<List<CommentResponseDto>>;
}
