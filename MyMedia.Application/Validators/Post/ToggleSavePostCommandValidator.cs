using FluentValidation;
using MyMedia.Application.Commands.Post;

namespace MyMedia.Application.Validators.Post
{
    internal sealed class ToggleSavePostCommandValidator : AbstractValidator<ToggleLikePostCommand>
    {
        public ToggleSavePostCommandValidator()
        {
            RuleFor(x => x.PostId).NotEmpty().WithMessage("PostId cannot be empty.");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty.");
        }
    }

}
