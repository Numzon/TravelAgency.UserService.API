namespace TravelAgency.UserService.Application.Common.Interfaces;
public interface IAmazonSimpleEmailService
{
    Task SendWelcomeEmailAsync(string email, CancellationToken cancellationToken);
}
