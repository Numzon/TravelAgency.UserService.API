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

    public async Task PublishUserForEmployeeCreated(string userId, int employeeId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var model = new UserForEmployeeCreatedPublishedDto { EmployeeId = employeeId, UserId = userId, Event = EventTypes.UserForEmployeeCreated };
        var message = JsonSerializer.Serialize(model);
        await _publisher.Publish(message);
    }
}
