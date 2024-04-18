using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;

namespace TravelAgency.UserService.Application.Authentication.Commands.ConfirmChangeEmail;
public sealed record ConfirmChangeEmailCommand(string AccessToken, string ConfirmationCode) : IResultRequest;

public sealed class ConfirmChangeEmailCommandHandler : IResultRequestHandler<ConfirmChangeEmailCommand>
{
    private readonly IAmazonCognitoService _cognitoService;

    public ConfirmChangeEmailCommandHandler(IAmazonCognitoService cognitoService)
    {
        _cognitoService = cognitoService;
    }

    public async Task<CustomResult> Handle(ConfirmChangeEmailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _cognitoService.ConfrimChangeEmailAsync(request, cancellationToken);
            return CustomResults.NoContent();
        }
        catch (Exception ex)
        {
            return CustomErrors.BadRequest(ex.Message);
        }
    }
}
