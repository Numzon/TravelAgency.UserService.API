using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;

namespace TravelAgency.UserService.Application.Authentication.Commands.ChangeUserAttributes;
public sealed record ChangeUserAttributesCommand(string AccessToken, string? GivenName, string? FamilyName, string? AgencyName) : IResultRequest;

public sealed class ChangeUserAttributesCommandHandler : IResultRequestHandler<ChangeUserAttributesCommand>
{
    private readonly IAmazonCognitoService _amazonService;

    public ChangeUserAttributesCommandHandler(IAmazonCognitoService amazonService)
    {
        _amazonService = amazonService;
    }
    public async Task<CustomResult> Handle(ChangeUserAttributesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _amazonService.ChangeUserAttributesAsync(request, cancellationToken);
            return CustomResults.NoContent();
        }
        catch (Exception ex)
        {
            return CustomErrors.BadRequest(ex.Message);
        }
    }
}
