using MediatR;
using MyMedia.Application.Interfaces;
using MyMedia.Application.Dtos.Response;
using MyMedia.Application.Queries.User;
using MyMedia.Shared.GlobalErrorHandling;
using AutoMapper;

namespace MyMedia.Application.Handlers.User
{
    public class GetFollowingsByUserIdQueryHandler : IRequestHandler<GetFollowingsByUserIdQuery, List<FollowingResponseDto>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetFollowingsByUserIdQueryHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<List<FollowingResponseDto>> Handle(GetFollowingsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserWithFollowDataAsync(request.UserId, cancellationToken);
            var currentUser = await _userService.GetUserWithFollowDataAsync(request.CurrentUserId, cancellationToken);

            if (user == null || currentUser == null)
                throw new CustomException("User not found.", 404);

            var followings = _mapper.Map<List<FollowingResponseDto>>(user.Followings);

            var currentUserFollowingIds = currentUser.Followings.Select(f => f.Id).ToHashSet();

            foreach (var dto in followings)
            {
                dto.IsFollowedByCurrentUser = currentUserFollowingIds.Contains(dto.Id);
            }

            return followings;
        }
    }
}
