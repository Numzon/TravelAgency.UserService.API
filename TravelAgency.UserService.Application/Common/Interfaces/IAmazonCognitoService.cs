﻿using TravelAgency.UserService.Application.Authentication.Commands.ChangeEmail;
using TravelAgency.UserService.Application.Authentication.Commands.ChangePassword;
using TravelAgency.UserService.Application.Authentication.Commands.ChangeUserAttributes;
using TravelAgency.UserService.Application.Authentication.Commands.ConfirmChangeEmail;
using TravelAgency.UserService.Application.Authentication.Commands.ConfirmForgotPassword;
using TravelAgency.UserService.Application.Authentication.Commands.RefreshToken;
using TravelAgency.UserService.Application.Authentication.Commands.SignIn;
using TravelAgency.UserService.Application.Authentication.Models;
using TravelAgency.UserService.Application.User.Commands.ConfirmUserCreation;
using TravelAgency.UserService.Application.User.Commands.CreateUser;
using TravelAgency.UserService.Application.User.Models;

namespace TravelAgency.UserService.Application.Common.Interfaces;
public interface IAmazonCognitoService
{
    Task<SignInResponseDto> SingInAsync(SignInCommand request, CancellationToken cancellationToken);
    Task<UserDto?> GetUserByIdAsync(string id, CancellationToken cancellationToken);
    Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenCommand command, CancellationToken cancellationToken);
    Task<SimpleUserDto?> GetSimpleUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<SimpleUserDto?> GetSimpleUserByIdAsync(string id, CancellationToken cancellationToken);
    Task ForgotPasswordAsync(string userId, CancellationToken cancellationToken);
    Task ConfirmForgotPasswordAsync(ConfirmForgotPasswordCommand command, CancellationToken cancellationToken);
    Task ChangePasswordAsync(ChangePasswordCommand request, CancellationToken cancellationToken);
    Task CreateUserAsync(CreateUserCommand command, CancellationToken cancellationToken);
    Task ConfirmUserCreationAsync(ConfirmUserCreationCommand command, CancellationToken cancellationToken);
    Task DeleteUserAsync(string email, CancellationToken cancellationToken);
    Task ChangeEmailAsync(ChangeEmailCommand command, CancellationToken cancellationToken);
    Task ConfrimChangeEmailAsync(ConfirmChangeEmailCommand command, CancellationToken cancellationToken);
    Task ChangeUserAttributesAsync(ChangeUserAttributesCommand command, CancellationToken cancellationToken);
}