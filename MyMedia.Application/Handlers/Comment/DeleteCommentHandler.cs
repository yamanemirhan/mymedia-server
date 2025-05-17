using MediatR;
using MyMedia.Application.Commands.Comment;
using MyMedia.Application.Common;
using MyMedia.Application.Interfaces;
using MyMedia.Shared.GlobalErrorHandling;

namespace MyMedia.Application.Handlers.Comment
{
    public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, Unit>
    {
        private readonly ICommentService _commentService;
        private readonly IAuthorizationHelper _authorizationHelper;

        public DeleteCommentHandler(
            ICommentService commentService,
            IAuthorizationHelper authorizationHelper)
        {
            _commentService = commentService;
            _authorizationHelper = authorizationHelper;
        }

        public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentService.GetCommentByIdAsync(request.CommentId);

            if (comment is null)
                throw new CustomException("Comment not found.", 404);

            await _authorizationHelper.EnsureUserIsOwnerOrAdminAsync(comment.UserId);

            await _commentService.DeleteCommentByIdAsync(comment.Id);
            return Unit.Value;
        }
    }
}
