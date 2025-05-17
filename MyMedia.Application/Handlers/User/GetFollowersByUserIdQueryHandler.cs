using AutoMapper;
using MediatR;
using MyMedia.Application.Dtos.Response;
using MyMedia.Application.Interfaces;
using MyMedia.Application.Queries.User;
using MyMedia.Shared.GlobalErrorHandling;

namespace MyMedia.Application.Handlers.User
{
    public class GetFollowersByUserIdQueryHandler : IRequestHandler<GetFollowersByUserIdQuery, List<FollowerResponseDto>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetFollowersByUserIdQueryHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<List<FollowerResponseDto>> Handle(GetFollowersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserWithFollowDataAsync(request.UserId, cancellationToken);
            var currentUser = await _userService.GetUserWithFollowDataAsync(request.CurrentUserId, cancellationToken);

            if (user == null || currentUser == null)
                throw new CustomException("User not found", 404);

            var followers = _mapper.Map<List<FollowerResponseDto>>(user.Followers);

            var followersId = new HashSet<Guid>(currentUser.Followers.Select(f => f.Id));

            foreach (var followerDto in followers)
            {
                followerDto.IsFollowingCurrentUser = followersId.Contains(followerDto.Id);
            }

            return followers;
        }
    }
}
