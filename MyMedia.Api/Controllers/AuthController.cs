using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Application.Commands.Auth;
using MyMedia.Application.Interfaces;
using MyMedia.Shared.GlobalErrorHandling;

namespace MyMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITokenService _tokenService;

        public AuthController(IMediator mediator, ITokenService tokenService)
        {
            _mediator = mediator;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            try
            {
                if (command == null)
                    return BadRequest("Invalid request.");

                var response = await _mediator.Send(command);
                return Ok(response);
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


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
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

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            try
            {
                var refreshToken = command.RefreshToken;

                if (string.IsNullOrWhiteSpace(refreshToken))
                    return BadRequest("No refresh token provided.");

                var response = await _mediator.Send(new RefreshTokenCommand(refreshToken));

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

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                string refreshToken = _tokenService.GetRefreshToken();

                if (string.IsNullOrWhiteSpace(refreshToken))
                    return BadRequest(new { error = "User is not logged in." });

                _tokenService.ClearTokens();

                return Ok(new { message = "Successfully logged out." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }
    }
}
