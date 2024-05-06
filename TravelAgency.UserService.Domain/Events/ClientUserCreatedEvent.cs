namespace TravelAgency.UserService.Domain.Events;
public sealed class ClientUserCreatedEvent : UserCreatedEvent
{
    public ClientUserCreatedEvent(string userId, string email, string firstName, string lastName) : base(userId)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }

    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}