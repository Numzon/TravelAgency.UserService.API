namespace TravelAgency.UserService.Domain.Events;
public sealed class ManagerUserCreatedEvent : UserCreatedEvent
{

    public ManagerUserCreatedEvent(string userId, string email, string firstName, string lastName, string group, int travelAgencyId) : base(userId)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Group = group;
        TravelAgencyId = travelAgencyId;
    }

    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Group { get; set; }
    public int TravelAgencyId { get; set; }
}
