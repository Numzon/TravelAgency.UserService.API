using FluentValidation;

namespace TravelAgency.UserService.Application.Authentication.Commands.RefreshToken;
public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}
