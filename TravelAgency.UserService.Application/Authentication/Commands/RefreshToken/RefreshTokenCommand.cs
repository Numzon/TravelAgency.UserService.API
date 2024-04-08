using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;

namespace TravelAgency.UserService.Application.Authentication.Commands.RefreshToken;
public sealed record RefreshTokenCommand(string RefreshToken) : IResultRequest;

public sealed class RefreshTokenCommandHandler : IResultRequestHandler<RefreshTokenCommand>
{
    private readonly IAmazonCognitoService _amazonService;

    public RefreshTokenCommandHandler(IAmazonCognitoService amazonService)
    {
        _amazonService = amazonService;
    }

    public async Task<CustomResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _amazonService.RefreshTokenAsync(request, cancellationToken);

            return CustomResults.Ok(result);
        }
        catch (Exception ex)
        {
            return CustomErrors.BadRequest(ex.Message);
        }
    }
}
