namespace TravelAgency.UserService.Application.Common.Models;
public class AmazonSimpleEmailServiceSettingsDto
{
    public required string Region { get; init; }
    public required string SourceEmailAddress { get; init; }
}
