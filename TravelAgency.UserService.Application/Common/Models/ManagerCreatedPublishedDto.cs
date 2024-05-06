using TravelAgency.SharedLibrary.Models;

namespace TravelAgency.UserService.Application.Common.Models;
public sealed class ManagerCreatedPublishedDto : BasePublishedDto
{
    public required int ManagerId { get; set; }
    public required string Email { get; set; }
    public required string Group { get; set; }
}
