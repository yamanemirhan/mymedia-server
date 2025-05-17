using FluentValidation;
using MyMedia.Application.Queries.User;

namespace MyMedia.Application.Validators.User
{
    internal sealed class GetUserByUsernameValidator : AbstractValidator<GetUserByUsernameQuery>
    {
        public GetUserByUsernameValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username is required.");
        }
    }
}
