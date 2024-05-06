using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using AutoMapper;
using Azure;
using Microsoft.Extensions.Options;
using System.Net;
using TravelAgency.SharedLibrary.Models;
using TravelAgency.UserService.Application.Authentication.Commands.SignIn;
using TravelAgency.UserService.Application.Authentication.Models;
using TravelAgency.UserService.Application.Common.Profiles;
using TravelAgency.UserService.Application.User.Models;
using TravelAgency.UserService.Domain.Enums;
using TravelAgency.UserService.Infrastructure.Services;

namespace TravelAgency.UserService.Infrastructure.IntegrationTests.Services;

public sealed class AmazonCognitoServiceTests
{
    private readonly Mock<IAmazonCognitoIdentityProvider> _provider;
    private readonly Mock<IOptions<AwsCognitoSettingsDto>> _options;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public AmazonCognitoServiceTests()
    {
        _provider = new Mock<IAmazonCognitoIdentityProvider>();
        _options = new Mock<IOptions<AwsCognitoSettingsDto>>();
        _fixture = new Fixture();

        var userProfile = new UserProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(userProfile));
        _mapper = new Mapper(configuration);
    }

    [Fact]
    public async Task SingInAsync_AwsSuccessResponse_ValidSignInResponse()
    {
        var command = new SignInCommand(_fixture.Create<string>(), _fixture.Create<string>());

        var response = _fixture.Create<InitiateAuthResponse>();

        _options.Setup(x => x.Value).Returns(GetRandomAwsCognitoSettingDto());
        _provider.Setup(x => x.InitiateAuthAsync(It.IsAny<InitiateAuthRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var service = new AmazonCognitoService(_provider.Object, _options.Object, _mapper);

        var result = await service.SingInAsync(command, default);

        result.Should().NotBeNull();
        result.GetType().Should().Be(typeof(SignInResponseDto));
        result.AuthResponse.Should().NotBeNull();
        result.AuthResponse!.AccessToken.Should().NotBeNullOrEmpty().And.Be(response.AuthenticationResult.AccessToken);
        result.AuthResponse!.RefreshToken.Should().NotBeNullOrEmpty().And.Be(response.AuthenticationResult.RefreshToken);
    }

    [Fact]
    public async Task GetUserByIdAsync_ValidId_UserTypeMappedToUserDto()
    {
        var id = _fixture.Create<string>();

        var response = new ListUsersResponse
        {
            Users = new List<UserType>
            {
                GetUserTypeWithGivenId(id)
            }
        };

        _options.Setup(x => x.Value).Returns(GetRandomAwsCognitoSettingDto());
        _provider.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var service = new AmazonCognitoService(_provider.Object, _options.Object, _mapper);

        var result = await service.GetUserByIdAsync(id, default);

        result.Should().NotBeNull();
        result!.GetType().Should().Be(typeof(UserDto));
        result.Id.Should().NotBeNullOrEmpty();
        result.Id.Should().Be(id);
    }

    [Fact]
    public async Task GetSimpleUserByIdAsync_ValidId_UserTypeMappedToUserDto()
    {
        var id = _fixture.Create<string>();

        var response = new ListUsersResponse
        {
            Users = new List<UserType>
            {
                GetUserTypeWithGivenId(id)
            }
        };

        _options.Setup(x => x.Value).Returns(GetRandomAwsCognitoSettingDto());
        _provider.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var service = new AmazonCognitoService(_provider.Object, _options.Object, _mapper);

        var result = await service.GetSimpleUserByIdAsync(id, default);

        result.Should().NotBeNull();
        result!.GetType().Should().Be(typeof(SimpleUserDto));
        result.Id.Should().NotBeNullOrEmpty();
        result.Id.Should().Be(id);
    }

    [Fact]
    public async Task GetSimpleUserByEmailAsync_ValidId_UserTypeMappedToUserDto()
    {
        var email = _fixture.Create<string>();

        var response = new ListUsersResponse
        {
            Users = new List<UserType>
            {
                GetUserTypeWithGivenEmail(email)
            }
        };

        _options.Setup(x => x.Value).Returns(GetRandomAwsCognitoSettingDto());
        _provider.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var service = new AmazonCognitoService(_provider.Object, _options.Object, _mapper);

        var result = await service.GetSimpleUserByEmailAsync(email, default);

        result.Should().NotBeNull();
        result!.GetType().Should().Be(typeof(SimpleUserDto));
        result.Email.Should().NotBeNullOrEmpty();
        result.Email.Should().Be(email);
    }

    [Fact]
    public async Task ForgotPasswordAsync_ValidRequest_ExceptionNotThrown()
    {
        var response = _fixture.Build<ForgotPasswordResponse>().With(x => x.HttpStatusCode, HttpStatusCode.NoContent).Create();

        _options.Setup(x => x.Value).Returns(GetRandomAwsCognitoSettingDto());
        _provider.Setup(x => x.ForgotPasswordAsync(It.IsAny<ForgotPasswordRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var service = new AmazonCognitoService(_provider.Object, _options.Object, _mapper);

        await service.Invoking(x => x.ForgotPasswordAsync(_fixture.Create<string>(), default)).Should().NotThrowAsync();
    }

    private AwsCognitoSettingsDto GetRandomAwsCognitoSettingDto()
    {
        return new AwsCognitoSettingsDto { ClientId = _fixture.Create<string>(), Region = _fixture.Create<string>(), UserPoolId = _fixture.Create<string>() };
    }

    private UserType GetUserTypeWithGivenId(string userId)
    {
        return new UserType
        {
            Attributes = new List<AttributeType>
            {
                new AttributeType
                {
                    Name = CognitoAttributes.Id,
                    Value = userId
                },
                new AttributeType
                {
                    Name = CognitoAttributes.Email,
                    Value = _fixture.Create<string>()
                },
                new AttributeType
                {
                    Name = CognitoAttributes.EmailVerified,
                    Value = bool.TrueString
                },
            }
        };
    }

    private UserType GetUserTypeWithGivenEmail(string email)
    {
        return new UserType
        {
            Attributes = new List<AttributeType>
            {
                new AttributeType
                {
                    Name = CognitoAttributes.Id,
                    Value = _fixture.Create<string>()
                },
                new AttributeType
                {
                    Name = CognitoAttributes.Email,
                    Value = email
                },
                new AttributeType
                {
                    Name = CognitoAttributes.EmailVerified,
                    Value = bool.TrueString
                },
            }
        };
    }
}
