using TravelAgency.SharedLibrary.Models;

namespace TravelAgency.UserService.Application.Common.Models;
public class UserForManagerCreatedPublishedDto : BasePublishedDto
{
    public required int ManagerId { get; set; }
    public required string UserId { get; set; }
}
