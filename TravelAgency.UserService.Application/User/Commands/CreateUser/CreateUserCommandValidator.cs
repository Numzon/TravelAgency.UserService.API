using FluentValidation;

namespace TravelAgency.UserService.Application.User.Commands.CreateUser;
public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty();

        RuleFor(x => x.Password)
            .Equal(x => x.ConfirmPassword);
    }
}
