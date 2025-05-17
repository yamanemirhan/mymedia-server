using MediatR;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Queries.User
{
    public record GetAllUsersQuery(Guid UserId) : IRequest<List<UserResponseDto>>;
}
