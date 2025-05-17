using MediatR;
using MyMedia.Application.Commands.Post;
using MyMedia.Application.Common;
using MyMedia.Application.Interfaces;
using MyMedia.Shared.GlobalErrorHandling;

namespace MyMedia.Application.Handlers.Post
{
    public class DeletePostHandler : IRequestHandler<DeletePostCommand, Unit>
    {
        private readonly IPostService _postService;
        private readonly IAuthorizationHelper _authorizationHelper;

        public DeletePostHandler(
            IPostService postService,
            IAuthorizationHelper authorizationHelper)
        {
            _postService = postService;
            _authorizationHelper = authorizationHelper;
        }

        public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postService.GetPostByIdAsync(request.PostId)
                       ?? throw new CustomException("Post not found.", 404);

            await _authorizationHelper.EnsureUserIsOwnerOrAdminAsync(post.UserId);

            await _postService.DeletePostByIdAsync(post.Id);
            return Unit.Value;
        }
    }
}
