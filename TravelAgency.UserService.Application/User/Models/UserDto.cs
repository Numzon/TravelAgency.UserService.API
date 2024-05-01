namespace TravelAgency.UserService.Application.User.Models;

public record UserDto
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? AgencyName { get; init; }
    public required bool IsDisabled { get; init; }
    public required bool IsVerified { get; init; }
};
