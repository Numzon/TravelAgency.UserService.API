using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgency.UserService.Domain.Common;
public abstract class BaseEntity
{
    public int Id { get; set; }

    private readonly IList<BaseEvent> _domainEvents = new List<BaseEvent>();

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvent()
    {
        _domainEvents.Clear();
    }
}
