using MediatR;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Queries.Post
{
    public record GetPostsByUserIdQuery(Guid UserId) : IRequest<List<PostResponseDto>>;

}
