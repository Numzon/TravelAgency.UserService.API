using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;

namespace TravelAgency.UserService.Application.Authentication.Commands.NewPassword;
public sealed record NewPasswordCommand(string Email, string Password, string Session) : IResultRequest;

public sealed class NewPasswordCommandHandler : IResultRequestHandler<NewPasswordCommand>
{
    private readonly IAmazonCognitoService _amazonService;

    public NewPasswordCommandHandler(IAmazonCognitoService amazonService)
    {
        _amazonService = amazonService;
    }

    public async Task<CustomResult> Handle(NewPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _amazonService.NewPasswordAsync(request, cancellationToken);

            return CustomResults.Ok(result);
        }
        catch (Exception ex)
        {
            return CustomErrors.BadRequest(ex.Message);
        }
    }
}
