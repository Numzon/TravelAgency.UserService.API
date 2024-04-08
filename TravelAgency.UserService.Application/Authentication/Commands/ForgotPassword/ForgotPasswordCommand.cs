using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;

namespace TravelAgency.UserService.Application.Authentication.Commands.ForgotPassword;
public record ForgotPasswordCommand(string Email) : IResultRequest;

public record ForgotPasswordCommandHandler : IResultRequestHandler<ForgotPasswordCommand>
{
    private readonly IAmazonCognitoService _amazonService;

    public ForgotPasswordCommandHandler(IAmazonCognitoService amazonService)
    {
        _amazonService = amazonService;
    }

    public async Task<CustomResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _amazonService.GetSimpleUserByEmailAsync(request.Email, cancellationToken);

            if (user is null)
            {
                return CustomErrors.BadRequest("User not found");
            }

            await _amazonService.ForgotPasswordAsync(user.Id, cancellationToken);

            return CustomResults.NoContent();
        }
        catch (Exception ex)
        {
            return CustomErrors.BadRequest(ex.Message);
        }

    }
}
