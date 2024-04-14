using System.Data.Common;

namespace TravelAgency.UserService.Application.IntegrationTests.TestDatabase.Interface;
public interface ITestDatabase
{
    Task InitialiseAsync();
    DbConnection GetConnection();
    Task ResetAsync();
    ValueTask DisposeAsync();
}
