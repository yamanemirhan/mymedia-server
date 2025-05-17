using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Application.Commands.Post;
using MyMedia.Application.Dtos.Request;
using MyMedia.Application.Queries.Post;
using MyMedia.Domain.Entities;
using MyMedia.Shared.GlobalErrorHandling;
using System.Security.Claims;

namespace MyMedia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequestDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Unauthorized(new { error = "Kullanıcı kimliği alınamadı." });
                }

                var postId = await _mediator.Send(new CreatePostCommand(dto, userId));
                return Ok(new { PostId = postId });
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
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPosts()
        {
            try
            {
                var query = new GetPostsQuery();
                var posts = await _mediator.Send(query);
                return Ok(posts);
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
        [HttpPost("{postId}/like")]
        public async Task<IActionResult> ToggleLikePost(Guid postId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Unauthorized(new { error = "Kullanıcı kimliği alınamadı." });
                }

                var command = new ToggleLikePostCommand(postId, userId);
                var result = await _mediator.Send(command);

                if (result)
                {
                    return Ok(new { message = "Post liked/unliked successfully" });
                }
                else
                {
                    return BadRequest(new { error = "An error occurred while toggling like." });
                }
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetSinglePostByIdQuery(id));
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
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPostsByUserId(Guid userId)
        {
            try
            {
                var result = await _mediator.Send(new GetPostsByUserIdQuery(userId));
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
        [HttpPost("{postId}/save")]
        public async Task<IActionResult> ToggleSavePost(Guid postId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Unauthorized(new { error = "Kullanıcı kimliği alınamadı." });
                }

                var command = new ToggleSavePostCommand(postId, userId);
                var result = await _mediator.Send(command);

                if (result)
                {
                    return Ok(new { message = "Post saved/unsaved successfully" });
                }
                else
                {
                    return BadRequest(new { error = "An error occurred while toggling save." });
                }
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
        [HttpGet("saved")]
        public async Task<IActionResult> GetSavedPosts()
        {
            try
            {

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Unauthorized(new { error = "Kullanıcı kimliği alınamadı." });
                }

                var command = new GetSavedPostsQuery(userId);
                var result = await _mediator.Send(command);
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
        [HttpDelete("{postId:guid}")]
        public async Task<IActionResult> DeletePost(Guid postId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Unauthorized(new { error = "Kullanıcı kimliği alınamadı." });
                }

                await _mediator.Send(new DeletePostCommand(postId, userId));
                return NoContent();
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
