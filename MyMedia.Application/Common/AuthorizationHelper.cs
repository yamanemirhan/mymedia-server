using Microsoft.AspNetCore.Http;
using MyMedia.Application.Interfaces;
using MyMedia.Shared.GlobalErrorHandling;
using System.Security.Claims;

namespace MyMedia.Application.Common
{
    public class AuthorizationHelper : IAuthorizationHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public AuthorizationHelper(IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public async Task EnsureUserIsOwnerOrAdminAsync(Guid resourceOwnerId)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null)
                throw new CustomException("User context not found.", 401);

            var userIdString = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var currentUserId))
                throw new CustomException("Invalid user ID.", 401);

            if (resourceOwnerId == currentUserId)
                return;

            var userEntity = await _userService.GetUserByIdAsync(currentUserId);
            if (userEntity == null || !userEntity.IsAdmin)
                throw new CustomException("You are not authorized to perform this action.", 403);
        }
    }
}
