namespace TravelAgency.UserService.Application.Common.Interfaces;
public interface IEmployeePublisher
{
    Task PublishUserForEmployeeCreated(string userId, int employeeId, CancellationToken cancellationToken);
}
