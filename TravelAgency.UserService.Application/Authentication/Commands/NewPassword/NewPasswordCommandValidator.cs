using FluentValidation;

namespace TravelAgency.UserService.Application.Authentication.Commands.NewPassword;
public sealed class NewPasswordCommandValidator : AbstractValidator<NewPasswordCommand>
{
	public NewPasswordCommandValidator()
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

        RuleFor(x => x.Session)
			.NotEmpty();
	}
}
