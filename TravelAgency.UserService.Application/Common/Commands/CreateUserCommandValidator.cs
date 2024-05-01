using FluentValidation;

namespace TravelAgency.UserService.Application.Common.Commands;
public abstract class CreateUserCommandValidator<TCreateUserCommand> : AbstractValidator<TCreateUserCommand>
    where TCreateUserCommand : CreateUserCommand
{
    protected CreateUserCommandValidator()
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
