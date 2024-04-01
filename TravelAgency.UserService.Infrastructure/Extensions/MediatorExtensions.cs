using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelAgency.UserService.Domain.Common;

namespace TravelAgency.UserService.Infrastructure.Extensions;
public static class MediatorExtensions
{
    public static async Task DispatchDomainEvents(this IMediator mediator, DbContext context)
    {
        var entries = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity);

        var domainEvents = entries
            .SelectMany(x => x.DomainEvents)
            .ToList();

        entries.ToList().ForEach(x => x.ClearDomainEvent());

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}
