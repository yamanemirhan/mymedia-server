using FluentValidation;
using MyMedia.Application.Queries.Post;

namespace MyMedia.Application.Validators.Post
{
    internal sealed class GetSinglePostByIdQueryValidator : AbstractValidator<GetSinglePostByIdQuery>
    {
        public GetSinglePostByIdQueryValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("Post ID is required.");
        }
    }
}
