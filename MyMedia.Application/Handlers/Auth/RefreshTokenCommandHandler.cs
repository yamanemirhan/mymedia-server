using MediatR;
using MyMedia.Application.Commands.Auth;
using MyMedia.Application.Dtos.Response;
using MyMedia.Application.Interfaces;
using MyMedia.Shared.GlobalErrorHandling;

namespace MyMedia.Application.Handlers.Auth
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        private readonly IAuthService _authService;

        public RefreshTokenCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                throw new CustomException("Refresh token is required.", 400);

            return await _authService.RefreshTokenAsync(request.RefreshToken);
        }
    }
}
