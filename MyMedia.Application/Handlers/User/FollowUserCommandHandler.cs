using MediatR;
using MyMedia.Application.Commands.User;
using MyMedia.Application.Interfaces;
using MyMedia.Shared.GlobalErrorHandling;
using System.Net;

namespace MyMedia.Application.Handlers.User
{
    public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand>
    {
        private readonly IUserService _userService;

        public FollowUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {
            if (request.CurrentUserId == request.TargetUserId)
                throw new CustomException("You cannot follow yourself.", (int)HttpStatusCode.BadRequest);

            await _userService.FollowUserAsync(request.CurrentUserId, request.TargetUserId, cancellationToken);
        }
    }
}
