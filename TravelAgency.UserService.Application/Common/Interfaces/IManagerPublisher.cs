using TravelAgency.UserService.Application.Common.Models;

namespace TravelAgency.UserService.Application.Common.Interfaces;
public interface IManagerPublisher
{
    Task PublishManagerCreated(ManagerPublishedDto user);
}
