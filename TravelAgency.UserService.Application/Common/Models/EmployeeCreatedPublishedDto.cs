using TravelAgency.SharedLibrary.Models;

namespace TravelAgency.UserService.Application.Common.Models;
public sealed class EmployeeCreatedPublishedDto : BasePublishedDto
{
    public required int EmployeeId { get; set; }
    public required string Email { get; set; }
}
