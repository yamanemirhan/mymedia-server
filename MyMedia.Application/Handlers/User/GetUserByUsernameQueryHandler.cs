using AutoMapper;
using MediatR;
using MyMedia.Application.Dtos.Response;
using MyMedia.Application.Interfaces;
using MyMedia.Application.Queries.User;
using MyMedia.Shared.GlobalErrorHandling;

namespace MyMedia.Application.Handlers.User
{
    public class GetUserByUsernameHandler : IRequestHandler<GetUserByUsernameQuery, UserResponseDto>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetUserByUsernameHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByUsernameAsync(request.Username);

            if (user == null)
                throw new CustomException("Kullanıcı bulunamadı.", 404);

            var currentUser = await _userService.GetUserWithFollowDataAsync(request.CurrentUserId, cancellationToken);

            var dto = _mapper.Map<UserResponseDto>(user);

            dto.IsFollowedByCurrentUser = user.Followers.Any(f => f.Id == currentUser.Id);
            dto.HasPendingFollowRequestFromCurrentUser = user.IncomingFollowRequests?.Any(r => r.RequesterId == currentUser.Id) ?? false;
            dto.HasSentFollowRequestToCurrentUser = user.OutgoingFollowRequests?.Any(r => r.ReceiverId == currentUser.Id) ?? false;
            dto.IsFollowingCurrentUser = user.Followings.Any(f => f.Id == currentUser.Id);

            return dto;
        }

    }
}
