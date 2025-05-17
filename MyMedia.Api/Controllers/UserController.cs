using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Application.Commands.User;
using MyMedia.Application.Dtos.Request.User;
using MyMedia.Application.Queries.User;
using MyMedia.Shared.GlobalErrorHandling;
using System.Security.Claims;

namespace MyMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { error = "User not authenticated." });

                var command = new GetCurrentUserQuery(Guid.Parse(userId));
                var response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(currentUserId))
                    return Unauthorized(new { error = "User not authenticated." });

                var result = await _mediator.Send(new GetUserByUserIdQuery(userId, Guid.Parse(currentUserId)));
                return Ok(result);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [Authorize]
        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(currentUserId))
                    return Unauthorized(new { error = "User not authenticated." });

                var result = await _mediator.Send(new GetUserByUsernameQuery(username, Guid.Parse(currentUserId)));
                return Ok(result);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [Authorize]
        [HttpPost("follow/{userId}")]
        public async Task<IActionResult> FollowUser(Guid userId)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(currentUserId))
                    return Unauthorized(new { error = "User not authenticated." });

                var command = new FollowUserCommand(Guid.Parse(currentUserId), userId);
                await _mediator.Send(command);
                return Ok(new { message = "Follow operation successful." });
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [Authorize]
        [HttpPost("unfollow/{userId}")]
        public async Task<IActionResult> UnfollowUser(Guid userId)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(currentUserId))
                    return Unauthorized(new { error = "User not authenticated." });

                var command = new UnfollowUserCommand(Guid.Parse(currentUserId), userId);
                await _mediator.Send(command);
                return Ok(new { message = "Unfollow operation successful." });
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [Authorize]
        [HttpPost("follow/{userId}/approve")]
        public async Task<IActionResult> ApproveFollowRequest(Guid userId)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(currentUserId))
                    return Unauthorized(new { error = "User not authenticated." });

                var command = new ApproveFollowRequestCommand(Guid.Parse(currentUserId), userId);
                await _mediator.Send(command);
                return Ok(new { message = "Follow request approved." });
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [Authorize]
        [HttpPost("follow/{requesterId}/reject")]
        public async Task<IActionResult> RejectFollowRequest(Guid requesterId)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(currentUserId))
                    return Unauthorized(new { error = "User not authenticated." });

                var command = new RejectFollowRequestCommand(Guid.Parse(currentUserId), requesterId);
                await _mediator.Send(command);

                return Ok(new { message = "Follow request rejected." });
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [Authorize]
        [HttpPost("follow/{receiverId}/cancel")]
        public async Task<IActionResult> CancelFollowRequest(Guid receiverId)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(currentUserId))
                    return Unauthorized(new { error = "User not authenticated." });

                var command = new CancelFollowRequestCommand(Guid.Parse(currentUserId), receiverId);
                await _mediator.Send(command);

                return Ok(new { message = "Follow request cancelled successfully." });
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [Authorize]
        [HttpPost("followers/{followerId}/remove")]
        public async Task<IActionResult> RemoveFollower(Guid followerId)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(currentUserId))
                    return Unauthorized(new { error = "User not authenticated." });

                var command = new RemoveFollowerCommand(Guid.Parse(currentUserId), followerId);
                await _mediator.Send(command);

                return Ok(new { message = "Follower removed successfully." });
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [Authorize]
        [HttpGet("{userId}/followers")]
        public async Task<IActionResult> GetFollowersByUserId(Guid userId)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(currentUserId))
                    return Unauthorized(new { error = "User not authenticated." });

                var query = new GetFollowersByUserIdQuery(userId, Guid.Parse(currentUserId));
                var followers = await _mediator.Send(query);

                return Ok(followers);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [Authorize]
        [HttpGet("{userId}/followings")]
        public async Task<IActionResult> GetFollowingsByUserId(Guid userId)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(currentUserId))
                    return Unauthorized(new { error = "User not authenticated." });

                var query = new GetFollowingsByUserIdQuery(userId, Guid.Parse(currentUserId));
                var followings = await _mediator.Send(query);

                return Ok(followings);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Unauthorized(new { error = "Kullanıcı kimliği alınamadı." });
                }

                var command = new UpdateUserCommand(
                    userId,
                    request.Username,
                    request.AvatarUrl,
                    request.Description,
                    request.IsPrivate
                );

                var updatedUser = await _mediator.Send(command);
                return Ok(updatedUser);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Unauthorized(new { error = "Kullanıcı kimliği alınamadı." });
                }

                var users = await _mediator.Send(new GetAllUsersQuery(userId));
                return Ok(users);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [Authorize]
        [HttpGet("follow-requests/incoming")]
        public async Task<IActionResult> GetIncomingFollowRequests()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                    return Unauthorized(new { error = "Kullanıcı kimliği alınamadı." });

                var result = await _mediator.Send(new GetIncomingFollowRequestsQuery(userId));
                return Ok(result);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { error = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [Authorize]
        [HttpGet("follow-requests/outgoing")]
        public async Task<IActionResult> GetOutgoingFollowRequests()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                    return Unauthorized(new { error = "Kullanıcı kimliği alınamadı." });

                var result = await _mediator.Send(new GetOutgoingFollowRequestsQuery(userId));
                return Ok(result);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { error = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }
    }
}