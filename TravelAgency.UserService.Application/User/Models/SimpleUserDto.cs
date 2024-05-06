namespace TravelAgency.UserService.Application.User.Models;
public sealed record SimpleUserDto
{
    public required string Id { get; init; }
    public required string Email { get; init; }
}