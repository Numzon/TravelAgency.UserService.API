using FluentValidation;

namespace TravelAgency.UserService.Application.Authentication.Commands.ConfirmForgotPassword;
public sealed class ConfirmForgotPasswordCommandValidator : AbstractValidator<ConfirmForgotPasswordCommand>
{
    public ConfirmForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.NewPassword)
            .NotEmpty();

        RuleFor(x => x.ConfirmationCode)
            .NotEmpty();
    }
}
