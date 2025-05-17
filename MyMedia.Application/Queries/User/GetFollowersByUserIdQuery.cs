using MediatR;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Queries.User
{
    public record GetFollowersByUserIdQuery(Guid UserId, Guid CurrentUserId) : IRequest<List<FollowerResponseDto>>;
}
