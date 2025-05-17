using MediatR;
using MyMedia.Application.Commands.User;
using MyMedia.Application.Interfaces;
using MyMedia.Shared.GlobalErrorHandling;
using System.Net;

namespace MyMedia.Application.Handlers.User
{
    public class ApproveFollowRequestCommandHandler : IRequestHandler<ApproveFollowRequestCommand>
    {
        private readonly IUserService _userService;

        public ApproveFollowRequestCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Handle(ApproveFollowRequestCommand request, CancellationToken cancellationToken)
        {
            if (request.CurrentUserId == request.RequesterId)
                throw new CustomException("You cannot approve your own follow request.", (int)HttpStatusCode.BadRequest);

            await _userService.ApproveFollowRequestAsync(request.CurrentUserId, request.RequesterId, cancellationToken);
        }
    }
}