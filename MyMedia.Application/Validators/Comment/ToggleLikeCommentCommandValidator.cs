using FluentValidation;
using MyMedia.Application.Commands.Comment;

namespace MyMedia.Application.Validators.Comment
{
    internal sealed class ToggleLikeCommentCommandValidator : AbstractValidator<ToggleLikeCommentCommand>
    {
        public ToggleLikeCommentCommandValidator()
        {
            RuleFor(x => x.CommentId)
                .NotEmpty().WithMessage("CommentId is required.");
        }
    }
}
