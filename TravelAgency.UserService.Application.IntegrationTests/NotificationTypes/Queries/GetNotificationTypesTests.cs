using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using TravelAgency.UserService.API.IntegrationTests;
using TravelAgency.UserService.Application.NotificationTypes.Models;
using TravelAgency.UserService.Application.NotificationTypes.Queries.GetNotificationTypesQuery;
using TravelAgency.UserService.Domain.Entities;
using TravelAgency.UserService.SharedTestLibrary.Helpers;

namespace TravelAgency.UserService.Application.IntegrationTests.NotificationTypes.Queries;

public class GetNotificationTypesTests : BaseSqlServerDbTest<NotificationType>
{
    public GetNotificationTypesTests() : base()
    {
    }


}
