using FluentValidation;

namespace TravelAgency.UserService.Application.User.Queries.GetUser;
public sealed class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
