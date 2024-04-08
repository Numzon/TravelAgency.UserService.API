using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;

namespace TravelAgency.UserService.Application.Authentication.Commands.ConfirmForgotPassword;
public sealed record ConfirmForgotPasswordCommand(string Email, string NewPassword, string ConfirmationCode) : IResultRequest;

public sealed class ConfirmForgotPasswordCommandHandler : IResultRequestHandler<ConfirmForgotPasswordCommand>
{
    private readonly IAmazonCognitoService _amazonService;

    public ConfirmForgotPasswordCommandHandler(IAmazonCognitoService amazonService)
    {
        _amazonService = amazonService;
    }

    public async Task<CustomResult> Handle(ConfirmForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _amazonService.ConfirmForgotPasswordAsync(request, cancellationToken);

            return CustomResults.NoContent();
        }
        catch (Exception ex)
        {
            return CustomErrors.BadRequest(ex.Message);
        }
    }
}
