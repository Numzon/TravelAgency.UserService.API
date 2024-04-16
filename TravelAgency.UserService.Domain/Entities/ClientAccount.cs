namespace TravelAgency.UserService.Domain.Entities;
public sealed class ClientAccount : BaseAuditableEntity
{
    public required string UserId { get; set; }

    public int? CreditCardId { get; set; }
    public CreditCard? CreditCard { get; set; }
}
