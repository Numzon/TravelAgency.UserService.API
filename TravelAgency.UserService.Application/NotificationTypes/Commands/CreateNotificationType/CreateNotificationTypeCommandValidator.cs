using FluentValidation;

namespace TravelAgency.UserService.Application.NotificationTypes.Commands.CreateNotificationType;
public sealed class CreateNotificationTypeCommandValidator : AbstractValidator<CreateNotificationTypeCommand>
{
	public CreateNotificationTypeCommandValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty();
	}
}
