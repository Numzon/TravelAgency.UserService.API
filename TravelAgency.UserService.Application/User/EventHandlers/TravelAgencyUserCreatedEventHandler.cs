using MediatR;
using Serilog;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Domain.Events;

namespace TravelAgency.UserService.Application.User.EventHandlers;
public sealed class TravelAgencyUserCreatedEventHandler : INotificationHandler<TravelAgencyUserCreatedEvent>
{
    private readonly ITravelAgencyPublisher _publisher;

    public TravelAgencyUserCreatedEventHandler(ITravelAgencyPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task Handle(TravelAgencyUserCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await _publisher.PublishTravelAgencyCreated(notification.UserId, notification.AgencyName, cancellationToken);
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }
}
