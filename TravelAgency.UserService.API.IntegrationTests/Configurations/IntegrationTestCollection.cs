using TravelAgency.UserService.Tests.Shared.Configurations;
using TravelAgency.UserService.Tests.Shared.Enums;

namespace TravelAgency.UserService.API.IntegrationTests.Configurations;

[CollectionDefinition(CollectionDefinitions.IntergrationTestCollection)]
public class IntegrationTestCollection : ICollectionFixture<TestContainerConfiguration>
{
    //configuration, no code here, class will not be ever created
}
