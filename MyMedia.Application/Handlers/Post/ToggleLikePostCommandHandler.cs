using MediatR;
using MyMedia.Application.Commands.Post;
using MyMedia.Application.Interfaces;
using MyMedia.Shared.GlobalErrorHandling;

namespace MyMedia.Application.Handlers.Post
{
    public class ToggleLikePostCommandHandler : IRequestHandler<ToggleLikePostCommand, bool>
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;

        public ToggleLikePostCommandHandler(IPostService postService, IUserService userService)
        {
            _postService = postService;
            _userService = userService;
        }

        public async Task<bool> Handle(ToggleLikePostCommand request, CancellationToken cancellationToken)
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

            var isLiked = post.LikedByUsers.Any(u => u.Id == request.UserId);

            if (isLiked)
            {
                post.LikedByUsers.Remove(user);
            }
            else
            {
                post.LikedByUsers.Add(user);
            }

            await _postService.SaveChangesAsync();

            return true;
        }
    }
}
