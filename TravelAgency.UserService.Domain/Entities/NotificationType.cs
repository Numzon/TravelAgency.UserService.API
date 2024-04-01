namespace TravelAgency.UserService.Domain.Entities;
public sealed class NotificationType : LookupEntity
{
    public ICollection<NotificationTemplate> NotificationTemplates { get; set; } = new List<NotificationTemplate>();
}
