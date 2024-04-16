using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Infrastructure.Persistance;
using TravelAgency.UserService.Infrastructure.Persistance.Interceptors;

namespace TravelAgency.UserService.SharedTestLibrary.BaseTests;
public abstract class BaseInMemoryDbTest : IAsyncLifetime
{
    protected readonly Fixture _fixture;
    protected UserServiceDbContext _dbContext;

    protected BaseInMemoryDbTest()
    {
        _fixture = new Fixture();
        _dbContext = null!;
    }

    public Task DisposeAsync()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        _dbContext = GetInMemoryDatabase();
        return Task.CompletedTask;
    }

    protected virtual UserServiceDbContext GetInMemoryDatabase()
    {
        var mediator = new Mock<IMediator>();
        var dateService = new Mock<IDateTimeService>();
        var currentUser = new Mock<ICurrentUserService>();

        dateService.Setup(x => x.Now).Returns(DateTime.Now);
        currentUser.Setup(x => x.AccessToken).Returns(_fixture.Create<string>());
        currentUser.Setup(x => x.Id).Returns(_fixture.Create<string>());

        var dbContextOptions = new DbContextOptionsBuilder<UserServiceDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
            .Options;

        var audiableInterceptor = new BaseAuditableEntitySaveChangesInterceptor(currentUser.Object,
            dateService.Object);

        return new UserServiceDbContext(dbContextOptions, mediator.Object, audiableInterceptor);
    }
}
