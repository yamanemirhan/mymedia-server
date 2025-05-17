using MediatR;
using MyMedia.Application.Commands.Auth;
using MyMedia.Application.Dtos.Response;
using MyMedia.Application.Interfaces;

namespace MyMedia.Application.Handlers.Auth
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponseDto>
    {
        private readonly IAuthService _authService;

        public LoginUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            return await _authService.LoginUserAsync(request.Email, request.Password);
        }
    }
}
