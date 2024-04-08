using FluentValidation;

namespace TravelAgency.UserService.Application.Authentication.Commands.ChangeUserAttributes;
public sealed class ChangeUserAttributesCommandValidator : AbstractValidator<ChangeUserAttributesCommand>
{
    public ChangeUserAttributesCommandValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty();
    }
}
