using MediatR;
using Serilog;
using TravelAgency.UserService.Domain.Events;

namespace TravelAgency.UserService.Application.ClientAccounts.EventHandlers;
public sealed class TravelAgencyUserCreatedEventHandler : INotificationHandler<TravelAgencyUserCreatedEvent>
{
    public async Task Handle(TravelAgencyUserCreatedEvent notification, CancellationToken cancellationToken)
    {
        Log.Information("Travel Agency for user");
    }
}
    