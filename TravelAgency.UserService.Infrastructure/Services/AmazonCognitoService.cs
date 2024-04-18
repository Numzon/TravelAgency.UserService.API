using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TravelAgency.SharedLibrary.Enums;
using TravelAgency.SharedLibrary.Models;
using TravelAgency.UserService.Application.Authentication.Commands.ChangeEmail;
using TravelAgency.UserService.Application.Authentication.Commands.ChangePassword;
using TravelAgency.UserService.Application.Authentication.Commands.ChangeUserAttributes;
using TravelAgency.UserService.Application.Authentication.Commands.ConfirmChangeEmail;
using TravelAgency.UserService.Application.Authentication.Commands.ConfirmForgotPassword;
using TravelAgency.UserService.Application.Authentication.Commands.RefreshToken;
using TravelAgency.UserService.Application.Authentication.Commands.SignIn;
using TravelAgency.UserService.Application.Authentication.Models;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.User.Commands.ConfirmUserCreation;
using TravelAgency.UserService.Application.User.Commands.CreateUser;
using TravelAgency.UserService.Application.User.Models;
using TravelAgency.UserService.Domain.Enums;

namespace TravelAgency.UserService.Infrastructure.Services;
public sealed class AmazonCognitoService : IAmazonCognitoService
{
    private readonly IAmazonCognitoIdentityProvider _client;
    private readonly AwsCognitoSettingsDto _settings;
    private readonly IMapper _mapper;

    public AmazonCognitoService(IAmazonCognitoIdentityProvider client, IOptions<AwsCognitoSettingsDto> options, IMapper mapper)
    {
        _settings = options.Value;
        _mapper = mapper;
        _client = client;
    }

    public async Task<SignInResponseDto> SingInAsync(SignInCommand request, CancellationToken cancellationToken)
    {
        var awsRequest = new InitiateAuthRequest
        {
            AuthFlow = AwsAuthFlows.UserPasswordAuth,
            AuthParameters = new Dictionary<string, string>
            {
                { AwsAuthParameters.Email, request.Email },
                { AwsAuthParameters.Password, request.Password }
            },
            ClientId = _settings.ClientId
        };

        var initiateAuthResponse = await _client.InitiateAuthAsync(awsRequest, cancellationToken);

        AuthenticationResultType authResult = initiateAuthResponse.AuthenticationResult;

        return new SignInResponseDto(authResult.AccessToken, authResult.RefreshToken);
    }

    public async Task<UserDto?> GetUserByIdAsync(string id, CancellationToken cancellationToken)
    {
        var user = await GetUserAsync($"{CognitoAttributes.Id} = \"{id}\"", cancellationToken);

        if (user is null)
        {
            return null!;
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task<SimpleUserDto?> GetSimpleUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await GetUserAsync($"{CognitoAttributes.Email} = \"{email}\"", cancellationToken, true);

        if (user is null)
        {
            return null!;
        }

        return _mapper.Map<SimpleUserDto>(user);
    }

    public async Task<SimpleUserDto?> GetSimpleUserByIdAsync(string id, CancellationToken cancellationToken)
    {
        var user = await GetUserAsync($"{CognitoAttributes.Id} = \"{id}\"", cancellationToken, true);

        if (user is null)
        {
            return null!;
        }

        return _mapper.Map<SimpleUserDto>(user);
    }

    public async Task ForgotPasswordAsync(string userId, CancellationToken cancellationToken)
    {
        var request = new ForgotPasswordRequest
        {
            ClientId = _settings.ClientId,
            Username = userId
        };

        await _client.ForgotPasswordAsync(request, cancellationToken);
    }

    public async Task ChangePasswordAsync(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var changePasswordRequest = new ChangePasswordRequest
        {
            AccessToken = request.AccessToken,
            PreviousPassword = request.PreviousPassword,
            ProposedPassword = request.ProposedPassword
        };

        await _client.ChangePasswordAsync(changePasswordRequest, cancellationToken);
    }

    public async Task CreateUserAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var userAttributes = GetUserAttributes(command);

        var signUpRequest = new SignUpRequest
        {
            ClientId = _settings.ClientId,
            Password = command.Password,
            UserAttributes = userAttributes,
            Username = command.Email
        };

        var addUserToGroupRequest = new AdminAddUserToGroupRequest
        {
            GroupName = command.IsTravelAgency ? CognitoGroups.TravelAgencyAccount : CognitoGroups.ClientAccount,
            Username = command.Email,
            UserPoolId = _settings.UserPoolId
        };

        await _client.SignUpAsync(signUpRequest, cancellationToken);
        await _client.AdminAddUserToGroupAsync(addUserToGroupRequest, cancellationToken);
    }

    public async Task ConfirmUserCreationAsync(ConfirmUserCreationCommand command, CancellationToken cancellationToken)
    {
        var request = new ConfirmSignUpRequest
        {
            ClientId = _settings.ClientId,
            ConfirmationCode = command.ConfirmationCode,
            Username = command.Email
        };

        await _client.ConfirmSignUpAsync(request, cancellationToken);
    }

    public async Task DeleteUserAsync(string email, CancellationToken cancellationToken)
    {
        var request = new AdminDeleteUserRequest
        {
            Username = email,
            UserPoolId = _settings.UserPoolId
        };

        await _client.AdminDeleteUserAsync(request, cancellationToken);
    }

    public async Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var request = new InitiateAuthRequest
        {
            AuthFlow = AwsAuthFlows.RefreshTokenAuth,
            AuthParameters = new Dictionary<string, string>
            {
                { AwsAuthParameters.RefreshToken, command.RefreshToken }
            },
            ClientId = _settings.ClientId
        };

        var initiateAuthResponse = await _client.InitiateAuthAsync(request, cancellationToken);

        AuthenticationResultType authResult = initiateAuthResponse.AuthenticationResult;

        return new RefreshTokenResponseDto(authResult.AccessToken);
    }

    public async Task ConfirmForgotPasswordAsync(ConfirmForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var request = new ConfirmForgotPasswordRequest
        {
            ClientId = _settings.ClientId,
            ConfirmationCode = command.ConfirmationCode,
            Password = command.NewPassword,
            Username = command.Email
        };

        await _client.ConfirmForgotPasswordAsync(request, cancellationToken);
    }

    public async Task ChangeEmailAsync(ChangeEmailCommand command, CancellationToken cancellationToken)
    {
        var request = new UpdateUserAttributesRequest
        {
            AccessToken = command.AccessToken,
            UserAttributes = new List<AttributeType>
            {
                new() { Name = CognitoAttributes.Email, Value = command.NewEmail }
            }
        };

        await _client.UpdateUserAttributesAsync(request, cancellationToken);
    }

    public async Task ChangeUserAttributesAsync(ChangeUserAttributesCommand command, CancellationToken cancellationToken)
    {
        var request = new UpdateUserAttributesRequest
        {
            AccessToken = command.AccessToken,
            UserAttributes = new List<AttributeType>
            {
                new() { Name = CognitoAttributes.GivenName, Value = command.GivenName ?? string.Empty },
                new() { Name = CognitoAttributes.FamilyName, Value = command.FamilyName ?? string.Empty },
                new() { Name = CognitoAttributes.AgencyName, Value = command.AgencyName ?? string.Empty },
            }
        };

        await _client.UpdateUserAttributesAsync(request, cancellationToken);
    }

    public async Task ConfrimChangeEmailAsync(ConfirmChangeEmailCommand command, CancellationToken cancellationToken)
    {
        var request = new VerifyUserAttributeRequest
        {
            AccessToken = command.AccessToken,
            AttributeName = CognitoAttributes.Email,
            Code = command.ConfirmationCode
        };

        await _client.VerifyUserAttributeAsync(request, cancellationToken);
    }

    public async Task<IEnumerable<string>> GetUserGroupsAsync(string userId, CancellationToken cancellationToken)
    {
        var request = new AdminListGroupsForUserRequest
        {
            Username = userId,
            UserPoolId = _settings.UserPoolId
        };

        var response = await _client.AdminListGroupsForUserAsync(request);

        if (response.Groups.IsNullOrEmpty())
        {
            return Enumerable.Empty<string>();  
        }

        return response.Groups.Select(x => x.GroupName);
    }

    private async Task<UserType?> GetUserAsync(string filter, CancellationToken cancellationToken, bool? isSimple = null)
    {
        var attributes = new List<string>();

        if (isSimple.HasValue && isSimple.Value)
        {
            attributes = new List<string>
            {
                CognitoAttributes.Id,
                CognitoAttributes.Email,
                CognitoAttributes.EmailVerified
            };
        }

        var request = new ListUsersRequest
        {
            AttributesToGet = attributes,
            Filter = filter,
            Limit = 1,
            UserPoolId = _settings.UserPoolId
        };

        var response = await _client.ListUsersAsync(request, cancellationToken);

        return response.Users.SingleOrDefault();
    }


    private List<AttributeType> GetUserAttributes(CreateUserCommand command)
    {
        if (command.IsTravelAgency)
        {
            return new List<AttributeType>
            {
                new() { Name = CognitoAttributes.AgencyName, Value = command.AgencyName }
            };
        }

        return new List<AttributeType>
        {
            new() { Name = CognitoAttributes.GivenName, Value = command.GivenName },
            new() { Name = CognitoAttributes.FamilyName, Value = command.FamilyName }
        };
    }
}
