using AutoMapper;
using MediatR;
using Serilog;
using TravelAgency.SharedLibrary.Enums;
using TravelAgency.SharedLibrary.Models;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Models;
using TravelAgency.UserService.Domain.Events;

namespace TravelAgency.UserService.Application.ClientAccounts.EventHandlers;
public sealed class TravelAgencyUserCreatedEventHandler : INotificationHandler<TravelAgencyUserCreatedEvent>
{
    private readonly ITravelAgencyPublisher _publisher;
    private readonly IMapper _mapper;

    public TravelAgencyUserCreatedEventHandler(ITravelAgencyPublisher publisher, IMapper mapper)
    {
        _publisher = publisher;
        _mapper = mapper;
    }

    public async Task Handle(TravelAgencyUserCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var userPublisher = _mapper.Map<TravelAgencyPublishedDto>(notification);
            userPublisher.Event = EventTypes.TravelAgencyUserCreated;
            await _publisher.PublishTravelAgencyCreated(userPublisher);
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }
}
    