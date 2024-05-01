using TravelAgency.UserService.Application.Common.Models;

namespace TravelAgency.UserService.Application.Common.Interfaces;
public interface IEmployeePublisher
{
    Task PublishEmployeeCreated(EmployeePublishedDto user);
}
