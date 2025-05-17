using AutoMapper;
using MediatR;
using MyMedia.Application.Dtos.Response;
using MyMedia.Application.Interfaces;
using MyMedia.Application.Queries.User;
using MyMedia.Application.Common;
using MyMedia.Shared.GlobalErrorHandling;

namespace MyMedia.Application.Handlers.User
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserResponseDto>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IAuthorizationHelper _authorizationHelper;

        public GetAllUsersQueryHandler(IUserService userService, IMapper mapper, IAuthorizationHelper authorizationHelper)
        {
            _userService = userService;
            _mapper = mapper;
            _authorizationHelper = authorizationHelper;
        }

        public async Task<List<UserResponseDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            await _authorizationHelper.EnsureUserIsOwnerOrAdminAsync(request.UserId);

            var users = await _userService.GetAllUsersAsync(cancellationToken);
            return _mapper.Map<List<UserResponseDto>>(users);
        }
    }
}
