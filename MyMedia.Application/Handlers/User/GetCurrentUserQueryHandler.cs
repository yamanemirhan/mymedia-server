using AutoMapper;
using MediatR;
using MyMedia.Application.Dtos.Response;
using MyMedia.Application.Interfaces;
using MyMedia.Application.Queries.User;
using MyMedia.Shared.GlobalErrorHandling;

namespace MyMedia.Application.Handlers.User
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserResponseDto>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetCurrentUserQueryHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByIdAsync(request.UserId);

            if (user == null)
                throw new CustomException("User not found.", 404);

            return _mapper.Map<UserResponseDto>(user);
        }
    }
}
