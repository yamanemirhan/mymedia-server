using FluentValidation;
using MyMedia.Application.Commands.User;

namespace MyMedia.Application.Validators.User
{
    internal sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("Description must not exceed 200 characters.");

            RuleFor(x => x.AvatarUrl)
                .MaximumLength(500).WithMessage("Avatar URL must not exceed 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.AvatarUrl));
        }
    }
}
