
using TravelAgency.UserService.Application.Common.Models;

namespace TravelAgency.UserService.Application.Common.Interfaces;
public interface ITravelAgencyPublisher
{
    Task PublishTravelAgencyCreated(TravelAgencyPublishedDto user);
}
