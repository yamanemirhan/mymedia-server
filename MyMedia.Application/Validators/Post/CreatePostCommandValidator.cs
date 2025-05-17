using FluentValidation;
using MyMedia.Application.Commands.Post;

namespace MyMedia.Application.Validators.Post
{
    internal sealed class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.PostDto.MediaItems)
                .Must(list => list != null && list.Count > 0)
                .WithMessage("At least one media item is required.");
        }
    }
}
