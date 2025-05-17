using AutoMapper;
using MediatR;
using MyMedia.Application.Commands.User;
using MyMedia.Application.Common;
using MyMedia.Application.Dtos.Response;
using MyMedia.Application.Interfaces;
using MyMedia.Domain.Entities;
using MyMedia.Shared.GlobalErrorHandling;

namespace MyMedia.Application.Handlers.User
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserResponseDto>
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationHelper _authorizationHelper;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(
            IUserService userService,
            IAuthorizationHelper authorizationHelper,
            IMapper mapper
        )
        {
            _userService = userService;
            _authorizationHelper = authorizationHelper;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await _authorizationHelper.EnsureUserIsOwnerOrAdminAsync(request.UserId);

            var user = await _userService.GetUserByIdAsync(request.UserId);

            if (user == null)
                throw new CustomException("User not found.", 404);

            var existingUser = await _userService.GetUserByUsernameAsync(request.Username);
            if (existingUser != null && existingUser.Id != request.UserId)
                throw new CustomException("This username is already taken.", 400);

            user.Username = request.Username;
            user.Description = request.Description;
            user.IsPrivate = request.IsPrivate;
            user.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(request.AvatarUrl))
            {
                if (user.Avatar == null)
                {
                    user.Avatar = new UserAvatarMedia
                    {
                        UserId = user.Id,
                        AvatarUrl = request.AvatarUrl
                    };
                }
                else
                {
                    user.Avatar.AvatarUrl = request.AvatarUrl;
                }
            }

            await _userService.UpdateUserAsync(user, cancellationToken);

            return _mapper.Map<UserResponseDto>(user);
        }
    }
}
