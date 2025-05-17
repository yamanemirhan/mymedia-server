using MediatR;
using MyMedia.Application.Commands.User;
using MyMedia.Application.Interfaces;
using MyMedia.Shared.GlobalErrorHandling;
using System.Net;

namespace MyMedia.Application.Handlers.User
{
    public class CancelFollowRequestCommandHandler : IRequestHandler<CancelFollowRequestCommand>
    {
        private readonly IUserService _userService;

        public CancelFollowRequestCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Handle(CancelFollowRequestCommand request, CancellationToken cancellationToken)
        {
            if (request.RequesterId == request.ReceiverId)
                throw new CustomException("You cannot cancel a follow request to yourself.", (int)HttpStatusCode.BadRequest);

            await _userService.CancelFollowRequestAsync(request.RequesterId, request.ReceiverId, cancellationToken);
        }
    }
}