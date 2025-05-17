using FluentValidation;
using MyMedia.Application.Commands.Comment;

namespace MyMedia.Application.Validators.Comment
{
    internal sealed class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty()
                .WithMessage("Text is required.");

            RuleFor(x => x.PostId)
                .NotEmpty()
                .WithMessage("PostId is required.");
        }
    }
}
