using MediatR;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Queries.User
{
    public record GetUserByUserIdQuery(Guid UserId, Guid CurrentUserId) : IRequest<UserResponseDto>;
}
