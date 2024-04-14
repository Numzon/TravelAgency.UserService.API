using FluentValidation;

namespace TravelAgency.UserService.Application.NotificationTypes.Commands.DeleteNotificationType;
public sealed class DeleteNotificationTypeCommandValidator : AbstractValidator<DeleteNotificationTypeCommand>
{
	public DeleteNotificationTypeCommandValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty();
	}
}
