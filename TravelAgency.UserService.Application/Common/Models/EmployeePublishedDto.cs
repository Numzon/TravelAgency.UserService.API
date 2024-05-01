using TravelAgency.SharedLibrary.Models;

namespace TravelAgency.UserService.Application.Common.Models;
public sealed class EmployeePublishedDto : BasePublishedDto
{
    public required string UserId { get; set; }
}
