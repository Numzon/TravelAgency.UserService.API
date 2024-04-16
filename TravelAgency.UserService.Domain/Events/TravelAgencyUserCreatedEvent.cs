namespace TravelAgency.UserService.Domain.Events;
public sealed class TravelAgencyUserCreatedEvent : UserCreatedEvent
{
    public TravelAgencyUserCreatedEvent(string userId) : base(userId)
    {

    }
}
