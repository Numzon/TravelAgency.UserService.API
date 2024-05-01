namespace TravelAgency.UserService.Domain.Events;
public sealed class TravelAgencyUserCreatedEvent : UserCreatedEvent
{
    public TravelAgencyUserCreatedEvent(string userId, string agencyName) : base(userId)
    {
        AgencyName = agencyName;
    }

    public string AgencyName { get; set; }
}
