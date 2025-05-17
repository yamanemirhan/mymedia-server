using MediatR;
using MyMedia.Application.Commands.User;
using MyMedia.Application.Interfaces;
using MyMedia.Shared.GlobalErrorHandling;
using System.Net;

namespace MyMedia.Application.Handlers.User
{
    public class RemoveFollowerCommandHandler : IRequestHandler<RemoveFollowerCommand, Unit>
    {
        private readonly IUserService _userService;

        public RemoveFollowerCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(RemoveFollowerCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _userService.GetUserWithFollowDataAsync(request.CurrentUserId, cancellationToken);

            if (currentUser == null)
                throw new CustomException("Not Found", 404);

            var follower = currentUser.Followers.FirstOrDefault(f => f.Id == request.FollowerId);

            if (follower == null)
                throw new CustomException("This user is not following you.", (int)HttpStatusCode.BadRequest);

            currentUser.Followers.Remove(follower);

            await _userService.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
