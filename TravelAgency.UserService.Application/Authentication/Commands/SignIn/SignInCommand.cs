using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;

namespace TravelAgency.UserService.Application.Authentication.Commands.SignIn;
public sealed record SignInCommand(string Email, string Password) : IResultRequest;

public sealed class SignInCommandHandler : IResultRequestHandler<SignInCommand>
{
    private readonly IAmazonCognitoService _amazonService;

    public SignInCommandHandler(IAmazonCognitoService amazonService)
    {
        _amazonService = amazonService;
    }

    public async Task<CustomResult> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _amazonService.SingInAsync(request, cancellationToken);

            return CustomResults.Ok(result);
        }
        catch (Exception ex)
        {
            return CustomErrors.BadRequest(ex.Message);
        }

    }
}
