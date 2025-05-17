using MediatR;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Queries.User
{
    public record GetIncomingFollowRequestsQuery(Guid UserId) : IRequest<List<FollowRequestResponseDto>>;
}
