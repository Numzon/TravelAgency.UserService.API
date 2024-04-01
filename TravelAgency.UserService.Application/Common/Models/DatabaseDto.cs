namespace TravelAgency.UserService.Application.Common.Models;

public sealed class DatabaseDto
{
    public string UserId { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string DataSource { get; set; } = null!;
    public string InitialCatalog { get; set; } = null!;
    public bool TrustServerCertificate { get; set; }
}
