namespace TravelAgency.UserService.Application.Common.Interfaces;
public interface ITravelAgencyPublisher
{
    Task PublishTravelAgencyCreated(string userId, string agencyName, CancellationToken cancellationToken);
}
