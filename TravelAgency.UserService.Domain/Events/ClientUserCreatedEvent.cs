namespace TravelAgency.UserService.Domain.Events;
public sealed class ClientUserCreatedEvent : UserCreatedEvent
{
	public ClientUserCreatedEvent(string userId) : base(userId)
	{

	}
}