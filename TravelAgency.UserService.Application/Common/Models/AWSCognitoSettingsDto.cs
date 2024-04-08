namespace TravelAgency.UserService.Application.Common.Models;
public sealed class AwsCognitoSettingsDto
{

    public required string ClientId { get; init; }
    public required string Region { get; init; }
    public required string UserPoolId { get; init; }

    public string AuthorityDiscoveryUrl => $"{AuthorityUrl}/.well-known/openid-configuration";
    public string AuthorityUrl => $"https://cognito-idp.{Region}.amazonaws.com/{UserPoolId}";
    public string JwtKeysUrl => $"{AuthorityUrl}/.well-known/jwks.json";
}
