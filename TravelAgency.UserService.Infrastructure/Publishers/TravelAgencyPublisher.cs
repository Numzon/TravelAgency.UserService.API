using System.Text.Json;
using TravelAgency.SharedLibrary.Models;
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

    public async Task PublishTravelAgencyCreated(TravelAgencyPublishedDto user)
    {
        var message = JsonSerializer.Serialize(user);
        await _publisher.Publish(message);
    }
}
