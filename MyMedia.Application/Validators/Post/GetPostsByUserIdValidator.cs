using FluentValidation;
using MyMedia.Application.Queries.Post;

namespace MyMedia.Application.Validators.Post
{
    internal sealed class GetPostsByUserIdValidator : AbstractValidator<GetPostsByUserIdQuery>
    {
        public GetPostsByUserIdValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId cannot be empty.");
        }
    }

}
