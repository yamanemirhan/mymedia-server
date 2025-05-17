using FluentValidation;
using MyMedia.Application.Commands.User;

namespace MyMedia.Application.Validators.User
{
    internal sealed class ApproveFollowRequestCommandValidator : AbstractValidator<ApproveFollowRequestCommand>
    {
        public ApproveFollowRequestCommandValidator()
        {
            RuleFor(x => x.RequesterId).NotEmpty().WithMessage("RequesterId is required.");
        }
    }
}
