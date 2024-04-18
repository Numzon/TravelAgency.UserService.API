namespace TravelAgency.UserService.Application.Common.Interfaces;
public interface ICurrentUserService
{
    string? AccessToken { get; }
    string? Id { get; }
}
