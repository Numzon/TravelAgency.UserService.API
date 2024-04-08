using FluentValidation;

namespace TravelAgency.UserService.Application.Authentication.Commands.ForgotPassword;
public sealed class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty();
    }
}
