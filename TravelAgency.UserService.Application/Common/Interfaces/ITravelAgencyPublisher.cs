using TravelAgency.SharedLibrary.Models;

namespace TravelAgency.UserService.Application.Common.Interfaces;
public interface ITravelAgencyPublisher
{
    Task PublishTravelAgencyCreated(TravelAgencyPublishedDto user);
}
