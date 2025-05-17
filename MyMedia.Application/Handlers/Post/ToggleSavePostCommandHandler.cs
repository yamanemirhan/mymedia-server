using MediatR;
using MyMedia.Application.Commands.Post;
using MyMedia.Application.Interfaces;
using MyMedia.Shared.GlobalErrorHandling;

namespace MyMedia.Application.Handlers.Post
{
    public class ToggleSavePostCommandHandler : IRequestHandler<ToggleSavePostCommand, bool>
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;

        public ToggleSavePostCommandHandler(IPostService postService, IUserService userService)
        {
            _postService = postService;
            _userService = userService;
        }

        public async Task<bool> Handle(ToggleSavePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postService.GetPostByIdAsync(request.PostId);
            if (post == null)
            {
                throw new CustomException("Post not found", 404);
            }

            var user = await _userService.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                throw new CustomException("User not found", 404);
            }

            var isSaved = post.SavedByUsers.Any(u => u.Id == request.UserId);

            if (isSaved)
            {
                post.SavedByUsers.Remove(user);
            }
            else
            {
                post.SavedByUsers.Add(user);
            }

            await _postService.SaveChangesAsync();

            return true;
        }
    }
}
