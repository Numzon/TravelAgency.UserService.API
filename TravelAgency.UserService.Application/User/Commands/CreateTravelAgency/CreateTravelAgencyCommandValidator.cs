using FluentValidation;
using TravelAgency.UserService.Application.Common.Commands;

namespace TravelAgency.UserService.Application.User.Commands.CreateTravelAgency;
public sealed class CreateTravelAgencyCommandValidator : CreateUserCommandValidator<CreateTravelAgencyCommand>
{
    public CreateTravelAgencyCommandValidator()
    {
        RuleFor(x => x.AgencyName)
            .NotEmpty();
    }
}
