using FluentValidation;

namespace TravelAgency.UserService.Application.Authentication.Commands.ConfirmChangeEmail;
public sealed class ConfirmChangeEmailCommandValidator : AbstractValidator<ConfirmChangeEmailCommand>
{
    public ConfirmChangeEmailCommandValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty();

        RuleFor(x => x.ConfirmationCode)
            .NotEmpty();
    }
}
