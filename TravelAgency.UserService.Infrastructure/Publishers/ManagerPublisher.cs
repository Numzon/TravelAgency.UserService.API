using System.Text.Json;
using TravelAgency.SharedLibrary.Enums;
using TravelAgency.SharedLibrary.RabbitMQ.Interfaces;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Models;

namespace TravelAgency.UserService.Infrastructure.Publishers;
public class ManagerPublisher : IManagerPublisher
{
    private readonly IMessageBusPublisher _publisher;

    public ManagerPublisher(IMessageBusPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task PublishUserForManagerCreated(string userId, int managerId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var model = new UserForManagerCreatedPublishedDto { Event = EventTypes.UserForManagerCreated, ManagerId = managerId, UserId = userId };
        var message = JsonSerializer.Serialize(model);
        await _publisher.Publish(message);
    }
}
