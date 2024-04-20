using Microsoft.EntityFrameworkCore;
using Serilog;

namespace TravelAgency.UserService.Infrastructure.Persistance;
public class UserServiceDbContextInitialiser
{
    private readonly UserServiceDbContext _userServiceDbContext;

    public UserServiceDbContextInitialiser(UserServiceDbContext userServiceDbContext)
    {
        _userServiceDbContext = userServiceDbContext;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_userServiceDbContext.Database.IsSqlServer())
            {
                await _userServiceDbContext.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            Log.Error("An error occurred while initialising the database.", ex);
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        try
        {
            await SeedAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }
    }

    public Task SeedAsync()
    {
        return Task.CompletedTask;
    }
}
