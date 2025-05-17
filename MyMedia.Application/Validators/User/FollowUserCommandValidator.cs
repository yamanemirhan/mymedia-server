using FluentValidation;
using MyMedia.Application.Commands.User;

namespace MyMedia.Application.Validators.User
{
    internal sealed class FollowUserCommandValidator : AbstractValidator<FollowUserCommand>
    {
        public FollowUserCommandValidator()
        {
            RuleFor(x => x.TargetUserId).NotEmpty();
        }
    }
}
