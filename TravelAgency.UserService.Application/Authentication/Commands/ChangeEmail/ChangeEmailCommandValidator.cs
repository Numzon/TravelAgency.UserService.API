using FluentValidation;

namespace TravelAgency.UserService.Application.Authentication.Commands.ChangeEmail;
public sealed class ChangeEmailCommandValidator : AbstractValidator<ChangeEmailCommand>
{
    public ChangeEmailCommandValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty();
        RuleFor(x => x.NewEmail)
            .NotEmpty();
    }
}
