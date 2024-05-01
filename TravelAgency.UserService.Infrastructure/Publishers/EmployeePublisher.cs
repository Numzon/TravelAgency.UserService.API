using System.Text.Json;
using TravelAgency.SharedLibrary.Enums;
using TravelAgency.SharedLibrary.RabbitMQ.Interfaces;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Models;

namespace TravelAgency.UserService.Infrastructure.Publishers;
public sealed class EmployeePublisher : IEmployeePublisher
{
    private readonly IMessageBusPublisher _publisher;

    public EmployeePublisher(IMessageBusPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task PublishEmployeeCreated(EmployeePublishedDto user)
    {
        user.Event = EventTypes.EmployeeCreated;
        var message = JsonSerializer.Serialize(user);
        await _publisher.Publish(message);
    }
}
