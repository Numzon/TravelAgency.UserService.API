using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace TravelAgency.UserService.Infrastructure.Persistance;
public class UserServiceDbContextInitialiser
{
    private readonly UserServiceDbContext _userServiceDbContext;
    private readonly IConfiguration _configuration;

    public UserServiceDbContextInitialiser(UserServiceDbContext userServiceDbContext, IConfiguration configuration)
    {
        _userServiceDbContext = userServiceDbContext;
        _configuration = configuration;
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
