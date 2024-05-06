using FluentValidation;

namespace TravelAgency.UserService.Application.User.Queries.GetUserByEmail;
public sealed class GetUserByEmailQueryValidator : AbstractValidator<GetUserByEmailQuery>
{
	public GetUserByEmailQueryValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty()
			.EmailAddress();
	}
}
