using FluentValidation;
using MyMedia.Application.Commands.Comment;

namespace MyMedia.Application.Validators.Comment
{
    internal sealed class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Comment text cannot be empty.")
                .MaximumLength(500).WithMessage("Comment text cannot exceed 500 characters.");
        }
    }
}
