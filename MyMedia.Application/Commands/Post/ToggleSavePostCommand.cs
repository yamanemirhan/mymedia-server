using MediatR;

namespace MyMedia.Application.Commands.Post
{
    public record ToggleSavePostCommand(Guid PostId, Guid UserId) : IRequest<bool>;
}
