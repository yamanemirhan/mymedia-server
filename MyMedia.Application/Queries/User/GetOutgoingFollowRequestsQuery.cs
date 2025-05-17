using MediatR;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Queries.User
{
    public record GetOutgoingFollowRequestsQuery(Guid UserId) : IRequest<List<FollowRequestResponseDto>>;
}
