using FluentValidation;

namespace TravelAgency.UserService.Application.Authentication.Commands.SignIn;
public sealed class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty();
        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
