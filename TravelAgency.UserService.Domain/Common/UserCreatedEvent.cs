namespace TravelAgency.UserService.Domain.Common;
public abstract class UserCreatedEvent : BaseEvent
{
    public string UserId { get; set; }

    protected UserCreatedEvent(string userId)
    {
        UserId = userId;
    }
}
