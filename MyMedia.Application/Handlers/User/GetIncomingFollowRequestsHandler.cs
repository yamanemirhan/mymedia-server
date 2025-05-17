using AutoMapper;
using MediatR;
using MyMedia.Application.Dtos.Response;
using MyMedia.Application.Interfaces;
using MyMedia.Application.Queries.User;

namespace MyMedia.Application.Handlers.User
{
    public class GetIncomingFollowRequestsHandler : IRequestHandler<GetIncomingFollowRequestsQuery, List<FollowRequestResponseDto>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetIncomingFollowRequestsHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<List<FollowRequestResponseDto>> Handle(GetIncomingFollowRequestsQuery request, CancellationToken cancellationToken)
        {
            var requests = await _userService.GetIncomingFollowRequestsAsync(request.UserId);
            return _mapper.Map<List<FollowRequestResponseDto>>(requests, opt => opt.Items["CurrentUserId"] = request.UserId);
        }
    }
}
