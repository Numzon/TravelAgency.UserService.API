using FluentValidation;
using TravelAgency.UserService.Application.Common.Commands;

namespace TravelAgency.UserService.Application.User.Commands.CreateManager;
public sealed class CreateManagerCommandValidator : CreateUserCommandValidator<CreateManagerCommand>
{
	public CreateManagerCommandValidator() : base()
	{
        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();

        RuleFor(x => x.Group)
            .NotEmpty();

        RuleFor(x => x.TravelAgencyId)
            .NotEmpty();
    }
}
