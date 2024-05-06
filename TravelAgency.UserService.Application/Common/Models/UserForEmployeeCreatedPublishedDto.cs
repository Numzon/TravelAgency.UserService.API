using TravelAgency.SharedLibrary.Models;

namespace TravelAgency.UserService.Application.Common.Models;
public class UserForEmployeeCreatedPublishedDto : BasePublishedDto
{
    public required string UserId { get; set; }
    public required int EmployeeId { get; set; }
}
