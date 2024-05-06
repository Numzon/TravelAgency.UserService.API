using TravelAgency.UserService.Application.Authentication.Commands.ChangeEmail;
using TravelAgency.UserService.Application.Authentication.Commands.ChangePassword;
using TravelAgency.UserService.Application.Authentication.Commands.ChangeUserAttributes;
using TravelAgency.UserService.Application.Authentication.Commands.ConfirmChangeEmail;
using TravelAgency.UserService.Application.Authentication.Commands.ConfirmForgotPassword;
using TravelAgency.UserService.Application.Authentication.Commands.NewPassword;
using TravelAgency.UserService.Application.Authentication.Commands.RefreshToken;
using TravelAgency.UserService.Application.Authentication.Commands.SignIn;
using TravelAgency.UserService.Application.Authentication.Models;
using TravelAgency.UserService.Application.Common.Models;
using TravelAgency.UserService.Application.User.Commands.ConfirmUserCreation;
using TravelAgency.UserService.Application.User.Commands.CreateClientAccount;
using TravelAgency.UserService.Application.User.Commands.CreateTravelAgency;
using TravelAgency.UserService.Application.User.Models;

namespace TravelAgency.UserService.Application.Common.Interfaces;
public interface IAmazonCognitoService
{
    Task<SignInResponseDto> SingInAsync(SignInCommand request, CancellationToken cancellationToken);
    Task<UserDto?> GetUserByIdAsync(string id, CancellationToken cancellationToken);
    Task<UserDto?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenCommand command, CancellationToken cancellationToken);
    Task<SimpleUserDto?> GetSimpleUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<SimpleUserDto?> GetSimpleUserByIdAsync(string id, CancellationToken cancellationToken);
    Task ForgotPasswordAsync(string userId, CancellationToken cancellationToken);
    Task ConfirmForgotPasswordAsync(ConfirmForgotPasswordCommand command, CancellationToken cancellationToken);
    Task ChangePasswordAsync(ChangePasswordCommand request, CancellationToken cancellationToken);
    Task<AuthResponseDto> NewPasswordAsync(NewPasswordCommand command, CancellationToken cancellationToken);
    Task CreateClientAccountAsync(CreateClientAccountCommand command, CancellationToken cancellationToken);
    Task CreateTravelAgencyAsync(CreateTravelAgencyCommand command, CancellationToken cancellationToken);
    Task ConfirmUserCreationAsync(ConfirmUserCreationCommand command, CancellationToken cancellationToken);
    Task DeleteUserAsync(string email, CancellationToken cancellationToken);
    Task ChangeEmailAsync(ChangeEmailCommand command, CancellationToken cancellationToken);
    Task ConfrimChangeEmailAsync(ConfirmChangeEmailCommand command, CancellationToken cancellationToken);
    Task ChangeUserAttributesAsync(ChangeUserAttributesCommand command, CancellationToken cancellationToken);
    Task<IEnumerable<string>> GetUserGroupsAsync(string userId, CancellationToken cancellationToken);
    Task<SimpleUserDto> CreateManagerAsync(CreateManagerDto manager, CancellationToken cancellationToken);
    Task<SimpleUserDto> CreateEmployeeAsync(string email, CancellationToken cancellationToken);
}
