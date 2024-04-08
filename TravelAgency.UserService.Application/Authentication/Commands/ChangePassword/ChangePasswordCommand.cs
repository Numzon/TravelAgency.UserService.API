using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;

namespace TravelAgency.UserService.Application.Authentication.Commands.ChangePassword;
public sealed record ChangePasswordCommand(string AccessToken, string PreviousPassword, string ProposedPassword) : IResultRequest;

public sealed class ChangePasswordCommandHandler : IResultRequestHandler<ChangePasswordCommand>
{
    private readonly IAmazonCognitoService _amazonService;

    public ChangePasswordCommandHandler(IAmazonCognitoService amazonService)
    {
        _amazonService = amazonService;
    }

    public async Task<CustomResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _amazonService.ChangePasswordAsync(request, cancellationToken);
            return CustomResults.Ok();
        }
        catch (Exception ex)
        {
            return CustomErrors.BadRequest(ex.Message);
        }
    }
}
