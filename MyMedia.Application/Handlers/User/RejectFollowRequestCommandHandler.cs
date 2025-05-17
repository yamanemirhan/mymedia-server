using MediatR;
using MyMedia.Application.Commands.User;
using MyMedia.Application.Interfaces;
using MyMedia.Shared.GlobalErrorHandling;
using System.Net;

namespace MyMedia.Application.Handlers.User
{
    public class RejectFollowRequestCommandHandler : IRequestHandler<RejectFollowRequestCommand>
    {
        private readonly IUserService _userService;

        public RejectFollowRequestCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Handle(RejectFollowRequestCommand request, CancellationToken cancellationToken)
        {
            if (request.ReceiverId == request.RequesterId)
                throw new CustomException("You cannot reject your own follow request.", (int)HttpStatusCode.BadRequest);

            await _userService.RejectFollowRequestAsync(request.ReceiverId, request.RequesterId, cancellationToken);
        }
    }
}