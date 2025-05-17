using FluentValidation;
using MyMedia.Application.Commands.Post;

namespace MyMedia.Application.Validators.Post
{
    internal sealed class DeletePostValidator : AbstractValidator<DeletePostCommand>
    {
        public DeletePostValidator()
        {
            RuleFor(x => x.PostId).NotEmpty().WithMessage("PostId is required.");
        }
    }
}
