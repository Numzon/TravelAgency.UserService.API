namespace TravelAgency.UserService.Application.Common.Models;
public class AmazonEmailServiceSettingsDto
{
    public required string Region { get; init; }
    public required string SourceEmailAddress { get; init; }
}
