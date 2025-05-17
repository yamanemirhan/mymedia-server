using FluentValidation;
using MyMedia.Application.Queries.User;

namespace MyMedia.Application.Validators.User
{
    internal sealed class GetUserByUserIdQueryValidator : AbstractValidator<GetUserByUserIdQuery>
    {
        public GetUserByUserIdQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        }
    }
}
