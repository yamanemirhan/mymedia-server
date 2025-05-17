using MediatR;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Queries.User
{
    public record GetCurrentUserQuery(Guid UserId) : IRequest<UserResponseDto>;
}
