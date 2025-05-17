using MediatR;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Queries.User
{
    public record GetFollowingsByUserIdQuery(Guid UserId, Guid CurrentUserId) : IRequest<List<FollowingResponseDto>>;
}
