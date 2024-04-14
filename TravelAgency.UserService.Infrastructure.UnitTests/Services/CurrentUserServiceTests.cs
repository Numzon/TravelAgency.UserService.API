using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Authentication;
using System.Security.Claims;
using TravelAgency.SharedLibrary.Enums;
using TravelAgency.UserService.Infrastructure.Services;

namespace TravelAgency.UserService.Infrastructure.IntegrationTests.Services;
public sealed class CurrentUserServiceTests
{
    private readonly Mock<IHttpContextAccessor> _accessor;
    private readonly Mock<IAuthenticationService> _authService;
    private readonly Mock<HttpContext> _httpContext;
    private readonly Mock<ClaimsPrincipal> _user;
    private readonly Fixture _fixture;

    public CurrentUserServiceTests()
    {
        _accessor = new Mock<IHttpContextAccessor>();
        _authService = new Mock<IAuthenticationService>();
        _httpContext = new Mock<HttpContext>();
        _user = new Mock<ClaimsPrincipal>();
        _fixture = new Fixture();

        _authService.Setup(x => x.AuthenticateAsync(_httpContext.Object, It.IsAny<string>())).ReturnsAsync(GetDummyAuthenticationResult());
        _httpContext.Setup(x => x.RequestServices.GetService(typeof(IAuthenticationService))).Returns(_authService.Object);
        _httpContext.Setup(x => x.User).Returns(_user.Object);
        _accessor.Setup(x => x.HttpContext).Returns(_httpContext.Object);
    }

    [Fact]
    public void Id_ValidUsernameClaim_ValidId()
    {
        var id = _fixture.Create<string>();

        _user.Setup(x => x.Claims).Returns(
          new[]
          {
                new Claim(AwsTokenNames.Username, id)
          });

        var service = new CurrentUserService(_accessor.Object);

        var retrivedId = service.Id;

        retrivedId.Should().NotBeNull();
        retrivedId.Should().Be(id);
    }


    [Fact]
    public void Id_ClaimsListIsEmpty_ThrowsAuthenticationException()
    {
        var service = new CurrentUserService(_accessor.Object);

        service.Invoking(x => x.Id).Should().Throw<AuthenticationException>().WithMessage("Invalid user id");
    }

    [Fact]
    public void AccessToken_TokenIsDefined_ValidAccessToken()
    {
        var accessToken = _fixture.Create<string>();

        var authResult = AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(), string.Empty));
        authResult.Properties!.StoreTokens(new[]
        {
            new AuthenticationToken { Name = AwsTokenNames.AccessToken, Value = accessToken }
        });

        _authService.Setup(x => x.AuthenticateAsync(_httpContext.Object, It.IsAny<string>())).ReturnsAsync(authResult);

        var service = new CurrentUserService(_accessor.Object);

        var retrivedAccessToken = service.AccessToken;

        retrivedAccessToken.Should().NotBeNull();
        retrivedAccessToken.Should().Be(accessToken);
    }

    [Fact]
    public void AccessToken_TokenIsNotDefined_ThrowsAuthenticationException()
    {
        var service = new CurrentUserService(_accessor.Object);

        service.Invoking(x => x.AccessToken).Should().Throw<AuthenticationException>().WithMessage("Invalid user access token");
    }

    private AuthenticateResult GetDummyAuthenticationResult()
    {
        return GetAuthenticationResult("dummy_token", "dummy_value");
    }

    private AuthenticateResult GetAuthenticationResult(string tokenName, string tokenValue)
    {
        var authResult = AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(), string.Empty));
        authResult.Properties!.StoreTokens(new[]
        {
            new AuthenticationToken { Name = tokenName, Value = tokenValue }
        });

        return authResult;
    }
}
