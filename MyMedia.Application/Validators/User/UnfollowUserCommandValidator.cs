using FluentValidation;
using MyMedia.Application.Commands.User;

namespace MyMedia.Application.Validators.User
{
    internal sealed class UnfollowUserCommandValidator : AbstractValidator<UnfollowUserCommand>
    {
        public UnfollowUserCommandValidator()
        {
            RuleFor(x => x.TargetUserId).NotEmpty();
        }
    }
}
