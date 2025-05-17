using MediatR;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Queries.Comment
{
    public record GetAllCommentsQuery(Guid UserId) : IRequest<List<CommentResponseDto>>;
}
