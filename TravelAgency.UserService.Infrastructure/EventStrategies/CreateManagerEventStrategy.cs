using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using TravelAgency.SharedLibrary.RabbitMQ.Interfaces;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Models;

namespace TravelAgency.UserService.Infrastructure.EventStrategies;
public class CreateManagerEventStrategy : IEventStrategy
{
    public async Task ExecuteEvent(IServiceScope scope, string message, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var manager = JsonSerializer.Deserialize<ManagerCreatedPublishedDto>(message);

        if (manager is null)
        {
            throw new InvalidOperationException("Manager object cannot be null");
        }

        var service = scope.ServiceProvider.GetRequiredService<IAmazonCognitoService>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        var publisher = scope.ServiceProvider.GetRequiredService<IManagerPublisher>();

        var createManager = mapper.Map<CreateManagerDto>(manager);

        var result = await service.CreateManagerAsync(createManager, cancellationToken);

        await publisher.PublishUserForManagerCreated(result.Id, manager.ManagerId, cancellationToken);
    }
}