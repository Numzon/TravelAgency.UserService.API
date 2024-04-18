using TravelAgency.SharedLibrary.Enums;
using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;
using TravelAgency.UserService.Application.User.Models;

namespace TravelAgency.UserService.Application.User.Commands.ConfirmUserCreation;
public sealed record ConfirmUserCreationCommand(string Email, string ConfirmationCode) : IResultRequest;

public sealed class ConfirmUserCreationCommandHandler : IResultRequestHandler<ConfirmUserCreationCommand>
{
    private readonly IAmazonCognitoService _cognitoService;
    private readonly IAmazonEmailService _emailService;
    private readonly IAmazonNotificationService _notificationService;

    public ConfirmUserCreationCommandHandler(
        IAmazonCognitoService cognitoService,
        IAmazonEmailService emailService,
        IAmazonNotificationService notificationService
        )
    {
        _cognitoService = cognitoService;
        _emailService = emailService;
        _notificationService = notificationService;
    }

    public async Task<CustomResult> Handle(ConfirmUserCreationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _cognitoService.GetSimpleUserByEmailAsync(request.Email, cancellationToken);

            if (user is null)
            {
                return CustomErrors.BadRequest();
            }

            await _cognitoService.ConfirmUserCreationAsync(request, cancellationToken);

            await SubscribeToNotificationTopicAsync(user, cancellationToken);

            //await _emailService.SendWelcomeEmailAsync(request.Email, cancellationToken);

            return CustomResults.NoContent();
        }
        catch (Exception ex)
        {
            return CustomErrors.BadRequest(ex.Message);
        }
    }

    private async Task SubscribeToNotificationTopicAsync(SimpleUserDto user, CancellationToken cancellationToken)
    {
        var groups = await _cognitoService.GetUserGroupsAsync(user.Id, cancellationToken);

        if (groups.Any(x => x == CognitoGroups.ClientAccount))
        {
            await _notificationService.SubscribeEmailForClientAccountAsync(user.Email, cancellationToken);
            return;
        }

        await _notificationService.SubscribeEmailForTravelAgencyAsync(user.Email, cancellationToken);
    }
}
