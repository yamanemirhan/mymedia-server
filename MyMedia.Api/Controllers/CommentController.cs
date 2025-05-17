using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Application.Commands.Comment;
using MyMedia.Application.Dtos.Request;
using MyMedia.Application.Dtos.Request.Comment;
using MyMedia.Application.Queries.Comment;
using MyMedia.Shared.GlobalErrorHandling;
using System.Security.Claims;

namespace MyMedia.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequestDto request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Unauthorized(new { error = "Kullanıcı kimliği alınamadı." });
                }

                var command = new CreateCommentCommand(
                    userId,
                    request.Text,
                    request.PostId,
                    request.ParentCommentId
                );

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

        [HttpGet("post/{postId}/comments")]
        public async Task<IActionResult> GetCommentsByPostId(Guid postId)
        {
            try
            {
                var query = new GetCommentsByPostIdQuery(postId);
                var comments = await _mediator.Send(query);
                return Ok(comments);
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

        [HttpPost("{commentId}/toggle-like")]
        public async Task<IActionResult> ToggleLike(Guid commentId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Unauthorized(new { error = "Kullanıcı kimliği alınamadı." });
                }

                await _mediator.Send(new ToggleLikeCommentCommand(commentId, userId));
                return NoContent();
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

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Unauthorized(new { error = "Kullanıcı kimliği alınamadı." });
                }

                await _mediator.Send(new DeleteCommentCommand(commentId, userId));
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

        [HttpPut("{commentId:guid}")]
        public async Task<IActionResult> UpdateCommentById(Guid commentId, [FromBody] UpdateCommentRequest request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Unauthorized(new { error = "Kullanıcı kimliği alınamadı." });
                }

                var command = new UpdateCommentCommand(commentId, userId, request.Text);
                await _mediator.Send(command);

                return Ok("Yorum başarıyla güncellendi.");
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
        public async Task<IActionResult> GetAllComments()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Unauthorized(new { error = "Kullanıcı kimliği alınamadı." });
                }

                var comments = await _mediator.Send(new GetAllCommentsQuery(userId));
                return Ok(comments);
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
    }
}
