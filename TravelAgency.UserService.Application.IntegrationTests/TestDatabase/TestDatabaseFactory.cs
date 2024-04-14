using TravelAgency.UserService.Application.IntegrationTests.TestDatabase.Interface;

namespace TravelAgency.UserService.Application.IntegrationTests.TestDatabase;
public static class TestDatabaseFactory
{
    public static async Task<ITestDatabase> CreateAsync()
    {
        var database = new SqlServerTestDatabase();

        await database.InitialiseAsync();

        return database;
    }
}