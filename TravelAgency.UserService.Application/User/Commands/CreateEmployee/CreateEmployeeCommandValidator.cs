using FluentValidation;
using TravelAgency.UserService.Application.Common.Commands;

namespace TravelAgency.UserService.Application.User.Commands.CreateEmployee;
public sealed class CreateEmployeeCommandValidator : CreateUserCommandValidator<CreateEmployeeCommand>
{
	public CreateEmployeeCommandValidator() : base()
	{
        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();
    }
}
