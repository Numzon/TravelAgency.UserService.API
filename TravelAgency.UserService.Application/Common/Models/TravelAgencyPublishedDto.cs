using TravelAgency.SharedLibrary.Models;

namespace TravelAgency.UserService.Application.Common.Models;
public class TravelAgencyPublishedDto : BasePublishedDto
{
    public required string UserId { get; set; }
    public required string AgencyName { get; set; }
}
