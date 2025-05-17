using MediatR;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Queries.Post
{
    public record GetPostsQuery : IRequest<List<PostResponseDto>>;

}
