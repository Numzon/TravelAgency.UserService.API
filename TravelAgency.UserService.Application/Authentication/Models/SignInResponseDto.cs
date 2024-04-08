using TravelAgency.UserService.Application.Common.Errors;

namespace TravelAgency.UserService.Application.Authentication.Models;

public record SignInResponseDto(string AccessToken, string RefreshToken);
