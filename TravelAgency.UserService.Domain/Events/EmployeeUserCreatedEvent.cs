namespace TravelAgency.UserService.Domain.Events;
public class EmployeeUserCreatedEvent : UserCreatedEvent
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public EmployeeUserCreatedEvent(string userId, string email, string firstName, string lastName) : base(userId)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}
