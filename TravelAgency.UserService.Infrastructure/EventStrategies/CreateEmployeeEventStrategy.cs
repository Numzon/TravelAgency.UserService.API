using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using TravelAgency.SharedLibrary.RabbitMQ.Interfaces;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Models;

namespace TravelAgency.UserService.Infrastructure.EventStrategies;

public class CreateEmployeeEventStrategy : IEventStrategy
{
    public async Task ExecuteEvent(IServiceScope scope, string message, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var employee = JsonSerializer.Deserialize<EmployeeCreatedPublishedDto>(message);

        if (employee is null)
        {
            throw new InvalidOperationException("Employee object cannot be null");
        }

        var service = scope.ServiceProvider.GetRequiredService<IAmazonCognitoService>();
        var publisher = scope.ServiceProvider.GetRequiredService<IEmployeePublisher>();

        var result = await service.CreateEmployeeAsync(employee.Email, cancellationToken);

        await publisher.PublishUserForEmployeeCreated(result.Id, employee.EmployeeId, cancellationToken);
    }
}