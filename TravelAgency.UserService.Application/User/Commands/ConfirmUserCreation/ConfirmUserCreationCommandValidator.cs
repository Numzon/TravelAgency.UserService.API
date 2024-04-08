using FluentValidation;

namespace TravelAgency.UserService.Application.User.Commands.ConfirmUserCreation;
public sealed class ConfirmUserCreationCommandValidator : AbstractValidator<ConfirmUserCreationCommand>
{
    public ConfirmUserCreationCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty();

        RuleFor(x => x.ConfirmationCode)
            .NotEmpty();
    }
}
