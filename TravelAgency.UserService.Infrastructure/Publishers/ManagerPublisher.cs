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

    public async Task PublishManagerCreated(ManagerPublishedDto user)
    {
        user.Event = EventTypes.ManagerCreated;
        var message = JsonSerializer.Serialize(user);
        await _publisher.Publish(message);
    }
}
