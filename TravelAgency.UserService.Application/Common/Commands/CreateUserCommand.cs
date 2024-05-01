namespace TravelAgency.UserService.Application.Common.Commands;
public abstract record CreateUserCommand(string Email, string Password, string ConfirmPassword);
