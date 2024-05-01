using AutoMapper;
using MediatR;
using Serilog;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Models;
using TravelAgency.UserService.Domain.Events;

namespace TravelAgency.UserService.Application.User.EventHandlers;
public sealed class ManagerUserCreatedEventHandler : INotificationHandler<ManagerUserCreatedEvent>
{
    private readonly IManagerPublisher _publisher;
    private readonly IMapper _mapper;

    public ManagerUserCreatedEventHandler(
        IManagerPublisher publisher, IMapper mapper)
    {
        _publisher = publisher;
        _mapper = mapper;
    }

    public async Task Handle(ManagerUserCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var userPublisher = _mapper.Map<ManagerPublishedDto>(notification);
            await _publisher.PublishManagerCreated(userPublisher);
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }
}
