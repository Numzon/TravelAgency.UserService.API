using FluentValidation;

namespace TravelAgency.UserService.Application.NotificationTypes.Commands.UpdateNotificationType;
public sealed class UpdateNotificationTypeCommandValidator : AbstractValidator<UpdateNotificationTypeCommand>
{
    public UpdateNotificationTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty();
    }
}
