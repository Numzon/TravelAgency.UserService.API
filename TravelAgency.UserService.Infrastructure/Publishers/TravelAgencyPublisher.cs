using System.Text.Json;
using TravelAgency.SharedLibrary.Enums;
using TravelAgency.SharedLibrary.RabbitMQ.Interfaces;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Models;

namespace TravelAgency.UserService.Infrastructure.Publishers;
public sealed class TravelAgencyPublisher : ITravelAgencyPublisher
{
    private readonly IMessageBusPublisher _publisher;

    public TravelAgencyPublisher(IMessageBusPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task PublishTravelAgencyCreated(string userId, string agencyName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var model = new TravelAgencyCreatedPublishedDto { UserId = userId, AgencyName = agencyName, Event = EventTypes.TravelAgencyUserCreated };
        var message = JsonSerializer.Serialize(model);
        await _publisher.Publish(message);
    }
}
