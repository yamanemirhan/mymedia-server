using MediatR;
using MyMedia.Application.Commands.Comment;
using MyMedia.Application.Common;
using MyMedia.Application.Interfaces;
using MyMedia.Shared.GlobalErrorHandling;

namespace MyMedia.Application.Handlers.Comment
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Unit>
    {
        private readonly ICommentService _commentService;
        private readonly IAuthorizationHelper _authorizationHelper;

        public UpdateCommentCommandHandler(
            ICommentService commentService,
            IAuthorizationHelper authorizationHelper)
        {
            _commentService = commentService;
            _authorizationHelper = authorizationHelper;
        }

        public async Task<Unit> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentService.GetCommentByIdAsync(request.CommentId)
                           ?? throw new CustomException("Comment not found.", 404);

            await _authorizationHelper.EnsureUserIsOwnerOrAdminAsync(comment.UserId);

            comment.Text = request.Text;
            comment.UpdatedAt = DateTime.UtcNow;
            comment.IsEdited = true;

            await _commentService.UpdateAsync(comment);

            return Unit.Value;
        }
    }
}
