using MediatR;
using MyMedia.Application.Dtos.Response;

namespace MyMedia.Application.Queries.User
{
    public record GetUserByUsernameQuery(string Username, Guid CurrentUserId) : IRequest<UserResponseDto>;
}
