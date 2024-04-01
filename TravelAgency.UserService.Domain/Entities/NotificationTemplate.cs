namespace TravelAgency.UserService.Domain.Entities;
public sealed class NotificationTemplate : BaseAuditableEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }

    public int NotificationTypeId { get; set; }
    public required NotificationType NotificationType { get; set; }
}
