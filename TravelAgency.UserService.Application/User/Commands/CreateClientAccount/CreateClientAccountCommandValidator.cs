using FluentValidation;
using TravelAgency.UserService.Application.Common.Commands;

namespace TravelAgency.UserService.Application.User.Commands.CreateClientAccount;
public sealed class CreateClientAccountCommandValidator : CreateUserCommandValidator<CreateClientAccountCommand>
{
    public CreateClientAccountCommandValidator() : base()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();
    }
}
