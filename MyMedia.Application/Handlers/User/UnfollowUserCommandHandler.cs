using MediatR;
using MyMedia.Application.Commands.User;
using MyMedia.Application.Interfaces;
using MyMedia.Shared.GlobalErrorHandling;
using System.Net;

namespace MyMedia.Application.Handlers.User
{
    public class UnfollowUserCommandHandler : IRequestHandler<UnfollowUserCommand>
    {
        private readonly IUserService _userService;

        public UnfollowUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
        {
            if (request.CurrentUserId == request.TargetUserId)
                throw new CustomException("You cannot unfollow yourself.", (int)HttpStatusCode.BadRequest);

            await _userService.UnfollowUserAsync(request.CurrentUserId, request.TargetUserId, cancellationToken);
        }
    }

}
