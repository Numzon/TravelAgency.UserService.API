using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;

namespace TravelAgency.UserService.Application.Authentication.Commands.ChangeEmail;
public sealed record ChangeEmailCommand(string AccessToken, string NewEmail) : IResultRequest;

public sealed class ChangeEmailCommandHandler : IResultRequestHandler<ChangeEmailCommand>
{
    private readonly IAmazonCognitoService _amazonService;

    public ChangeEmailCommandHandler(IAmazonCognitoService amazonService)
    {
        _amazonService = amazonService;
    }

    public async Task<CustomResult> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _amazonService.ChangeEmailAsync(request, cancellationToken);

            return CustomResults.NoContent();
        }
        catch (Exception ex)
        {
            return CustomErrors.BadRequest(ex.Message);
        }
    }
}
