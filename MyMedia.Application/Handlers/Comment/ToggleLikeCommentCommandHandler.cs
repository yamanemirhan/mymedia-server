using MediatR;
using MyMedia.Application.Commands.Comment;
using MyMedia.Application.Interfaces;
using MyMedia.Shared.GlobalErrorHandling;

namespace MyMedia.Application.Handlers.Comment
{
    public class ToggleLikeCommentCommandHandler : IRequestHandler<ToggleLikeCommentCommand, Unit>
    {
            private readonly ICommentService _commentService;
            private readonly IUserService _userService;

            public ToggleLikeCommentCommandHandler(ICommentService commentService, IUserService userService)
            {
                _commentService = commentService;
                _userService = userService;
            }

            public async Task<Unit> Handle(ToggleLikeCommentCommand request, CancellationToken cancellationToken)
            {
                var user = await _userService.GetUserByIdAsync(request.UserId);
                if (user == null)
                {
                    throw new CustomException("User not found", 404);
                }

                await _commentService.ToggleLikeCommentAsync(request.CommentId, request.UserId, cancellationToken);
                return Unit.Value;
            }
    }
}
