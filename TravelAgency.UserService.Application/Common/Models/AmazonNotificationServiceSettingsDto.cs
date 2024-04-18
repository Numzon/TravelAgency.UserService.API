namespace TravelAgency.UserService.Application.Common.Models;
public class AmazonNotificationServiceSettingsDto
{
    public required string Region { get; init; }
    public required string ClientArn { get; set; }
    public required string TravelAgencyArn { get; set; }

}
