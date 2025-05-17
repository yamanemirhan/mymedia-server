using MediatR;
using MyMedia.Application.Dtos.Request;

namespace MyMedia.Application.Commands.Post
{
    public record CreatePostCommand(CreatePostRequestDto PostDto, Guid UserId) : IRequest<Guid>;
}
