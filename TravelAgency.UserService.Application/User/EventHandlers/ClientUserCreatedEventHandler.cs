using MediatR;
using Serilog;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Domain.Events;

namespace TravelAgency.UserService.Application.User.EventHandlers;
public sealed class ClientUserCreatedEventHandler : INotificationHandler<ClientUserCreatedEvent>
{
    private readonly IClientAccountRepository _repository;

    public ClientUserCreatedEventHandler(IClientAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(ClientUserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var exists = await _repository.DoesClientAccountForGivenUserExist(notification.UserId, cancellationToken);

        if (exists is false)
        {
            await _repository.CreateAsync(notification, cancellationToken);
            Log.Information($"Client Account for user with id: {notification.UserId} has been created");
            return;

        }
        Log.Error($"User with id: {notification.UserId} already exists");
    }
}
