using MediatR;
using MyMedia.Application.Commands.Auth;
using MyMedia.Application.Dtos.Response;
using MyMedia.Application.Interfaces;

namespace MyMedia.Application.Handlers.Auth
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponseDto>
    {
        private readonly IAuthService _authService;

        public RegisterUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var response = await _authService.RegisterUserAsync(
                request.Username,
                request.Email,
                request.Password
            );

            return response;
        }
    }
}
