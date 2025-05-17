using MediatR;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Queries.Post
{
    public record GetSavedPostsQuery(Guid UserId) : IRequest<List<PostResponseDto>>;
}