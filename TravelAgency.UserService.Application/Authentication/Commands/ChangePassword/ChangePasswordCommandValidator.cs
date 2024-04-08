using FluentValidation;

namespace TravelAgency.UserService.Application.Authentication.Commands.ChangePassword;
public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty();
        RuleFor(x => x.ProposedPassword)
            .NotEmpty();
        RuleFor(x => x.PreviousPassword)
            .NotEmpty();
    }
}
