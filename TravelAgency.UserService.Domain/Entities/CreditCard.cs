namespace TravelAgency.UserService.Domain.Entities;
public sealed class CreditCard : BaseAuditableEntity
{
    public required string CardNumber { get; set; }
    public required string Code { get; set; }
    public required DateTime ExpirationDate { get; set; }

    public int ClientAccountId { get; set; }
    public required ClientAccount ClientAccount { get; set; }
}
