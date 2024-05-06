namespace TravelAgency.UserService.Application.Common.Interfaces;
public interface IManagerPublisher
{
    Task PublishUserForManagerCreated(string userId, int managerId, CancellationToken cancellationToken);
}
