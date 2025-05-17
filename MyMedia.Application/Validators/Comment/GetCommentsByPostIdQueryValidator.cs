using FluentValidation;
using MyMedia.Application.Queries.Comment;

namespace MyMedia.Application.Validators.Comment
{
    internal sealed class GetCommentsByPostIdQueryValidator : AbstractValidator<GetCommentsByPostIdQuery>
    {
        public GetCommentsByPostIdQueryValidator()
        {
            RuleFor(x => x.PostId).NotEmpty().WithMessage("Post ID is required.");
        }
    }

}
