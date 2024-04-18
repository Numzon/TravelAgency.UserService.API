namespace TravelAgency.UserService.Application.Common.Interfaces;
public interface IAmazonEmailService
{
    Task SendWelcomeEmailAsync(string email, CancellationToken cancellationToken);
}
