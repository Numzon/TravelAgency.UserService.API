using Cysharp.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Mime;
using System.Text;
using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.NotificationTypes.Commands.CreateNotificationType;
using TravelAgency.UserService.Application.NotificationTypes.Commands.UpdateNotificationType;
using TravelAgency.UserService.Application.NotificationTypes.Models;
using TravelAgency.UserService.Application.NotificationTypes.Queries.GetNotificationTypesQuery;
using TravelAgency.UserService.Domain.Entities;
using TravelAgency.UserService.Infrastructure.Persistance;
using TravelAgency.UserService.Tests.Shared.Configurations;
using TravelAgency.UserService.Tests.Shared.Enums;
using TravelAgency.UserService.Tests.Shared.Helpers;

namespace TravelAgency.UserService.API.IntegrationTests.Controllers;

[Collection(CollectionDefinitions.IntergrationTestCollection)]
public sealed class NotificationTypeControllerTests : BaseIntegrationTest
{
    [Fact]
    public async Task Create_ValidTypeName_CreatesAndReturnsNotificationType()
    {
        using (TestServer = HostConfiguration.Build().Server)
        {
            //Initialisation
            await InitializeDatabaseAsync();

            //Arrange
            var command = _fixture.Create<CreateNotificationTypeCommand>();
            using HttpClient httpClient = TestServer.CreateClient();

            //Act
            HttpResponseMessage httpResponse = await httpClient.PostAsync($"/api/notification-types",
                new StringContent(JsonConvert.SerializeObject(command, new StringEnumConverter()), Encoding.UTF8, MediaTypeNames.Application.Json));

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<NotificationTypeDto>(content);

            response.Should().NotBeNull();
            response!.Id.Should().BeGreaterThan(0);
            response.Name.Should().Be(command.Name);

            //Cleanup
            await ResetDatabaseAsync();
        }
    }

    [Fact]
    public async Task Create_InvalidName_ErrorNoActionCompleted()
    {
        using (TestServer = HostConfiguration.Build().Server)
        {
            //Initialisation
            await InitializeDatabaseAsync();

            //Arrange
            var command = new CreateNotificationTypeCommand(null!);
            using HttpClient httpClient = TestServer.CreateClient();

            //Act
            HttpResponseMessage httpResponse = await httpClient.PostAsync($"/api/notification-types",
                    new StringContent(JsonConvert.SerializeObject(command, new StringEnumConverter()), Encoding.UTF8, MediaTypeNames.Application.Json));

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await httpResponse.Content.ReadAsStringAsync();

            content.Should().NotBeNull();
            //cleanup
            await ResetDatabaseAsync();
        }
    }

    [Fact]
    public async Task Create_SendedNameIsNullOrEmpty_ValidationError()
    {
        using (TestServer = HostConfiguration.Build().Server)
        {
            //Initialisation
            await InitializeDatabaseAsync();

            //Arrange
            var command = _fixture.Build<CreateNotificationTypeCommand>().With(x => x.Name, string.Empty).Create();
            using HttpClient httpClient = TestServer.CreateClient();

            //Act
            HttpResponseMessage httpResponse = await httpClient.PostAsync($"/api/notification-types",
                    new StringContent(JsonConvert.SerializeObject(command, new StringEnumConverter()), Encoding.UTF8, MediaTypeNames.Application.Json));

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ValidationError>(content);

            response.Should().NotBeNull();
            response!.Message.Should().NotBeNull();
            response.Failures.Should().NotBeNull();
            response.Failures.Should().HaveCountGreaterThan(0);

            //Cleanup
            await ResetDatabaseAsync();
        }
    }

    [Fact]
    public async Task Delete_EntityWithGivenIdDoestExist_NotFound()
    {
        using (TestServer = HostConfiguration.Build().Server)
        {
            //Initialisation
            await InitializeDatabaseAsync();

            //Arrange
            using HttpClient httpClient = TestServer.CreateClient();
            var id = _fixture.Create<int>();

            //Act
            HttpResponseMessage httpResponse = await httpClient.DeleteAsync($"/api/notification-types/{id}");

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<NotFoundError<int>>(content);

            response.Should().NotBeNull();
            response!.Message.Should().NotBeNullOrEmpty();

            //Cleanup
            await ResetDatabaseAsync();
        }
    }


    [Fact]
    public async Task Delete_EntityExists_EntityDeleted()
    {
        using (TestServer = HostConfiguration.Build().Server)
        {
            //Initialisation
            await InitializeDatabaseAsync();

            //Arrange
            using HttpClient httpClient = TestServer.CreateClient();
            using IServiceScope scope = TestServer.Services.CreateScope();

            var context = scope.ServiceProvider.GetService<IUserServiceDbContext>()!;
            var typeToDelete = new NotificationType { Name = _fixture.Create<string>() };
            var randomType = new NotificationType { Name = _fixture.Create<string>() };

            await context.NotificationTypes.AddAsync(typeToDelete);
            await context.NotificationTypes.AddAsync(randomType);
            await context.SaveChangesAsync();

            //Act
            HttpResponseMessage httpResponse = await httpClient.DeleteAsync($"/api/notification-types/{typeToDelete.Id}");

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var exists = await context.NotificationTypes.AnyAsync(x => x.Id == typeToDelete.Id);

            var typesCount = await context.NotificationTypes.CountAsync();

            exists.Should().BeFalse();
            typesCount.Should().Be(1);

            //Cleanup
            await ResetDatabaseAsync();
        }
    }

    [Fact]
    public async Task Delete_SendedIdEqualsZero_ValidationError()
    {
        using (TestServer = HostConfiguration.Build().Server)
        {
            //Initialisation
            await InitializeDatabaseAsync();

            //Arrange
            using HttpClient httpClient = TestServer.CreateClient();

            //Act
            HttpResponseMessage httpResponse = await httpClient.DeleteAsync($"/api/notification-types/{0}");

            //Assess
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var response = await httpResponse.Content.ReadAsStringAsync();
            var validationError = JsonConvert.DeserializeObject<ValidationError>(response);

            validationError.Should().NotBeNull();
            validationError!.Message.Should().NotBeNullOrEmpty();

            //Cleanup
            await ResetDatabaseAsync();
        }
    }

    [Fact]
    public async Task Update_EntityDoesntExist_NotFoundError()
    {
        using (TestServer = HostConfiguration.Build().Server)
        {
            //Initialisation
            await InitializeDatabaseAsync();

            //Arrange
            var command = _fixture.Build<UpdateNotificationTypeCommand>().With(x => x.Name, _fixture.Create<string>()).Create();
            using HttpClient httpClient = TestServer.CreateClient();

            //Act
            HttpResponseMessage httpResponse = await httpClient.PutAsync($"/api/notification-types",
                    new StringContent(JsonConvert.SerializeObject(command, new StringEnumConverter()), Encoding.UTF8, MediaTypeNames.Application.Json));

            //Assess
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var content = await httpResponse.Content.ReadAsStringAsync();
            var notFound = JsonConvert.DeserializeObject<NotFoundError<int>>(content);

            notFound.Should().NotBeNull();
            notFound!.Message.Should().NotBeNull();

            //Cleanup
            await ResetDatabaseAsync();
        }
    }

    [Fact]
    public async Task Update_EntityExists_EntityUpdated()
    {
        using (TestServer = HostConfiguration.Build().Server)
        {
            //Initialisation
            await InitializeDatabaseAsync();

            //Arrange
            using HttpClient httpClient = TestServer.CreateClient();
            using IServiceScope scope = TestServer.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<UserServiceDbContext>()!;

            var entity = new NotificationType { Name = _fixture.Create<string>() };
            await context.NotificationTypes.AddAsync(entity);
            await context.SaveChangesAsync();

            context.ChangeTracker.Clear();

            var command = _fixture.Build<UpdateNotificationTypeCommand>().With(x => x.Id, entity.Id).Create();
            //Act
            HttpResponseMessage httpResponse = await httpClient.PutAsync($"/api/notification-types",
                    new StringContent(JsonConvert.SerializeObject(command, new StringEnumConverter()), Encoding.UTF8, MediaTypeNames.Application.Json));

            //Assess
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var entityAfterUpdate = await context.NotificationTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id);

            entityAfterUpdate.Should().NotBeNull();
            entityAfterUpdate!.Name.Should().Be(command.Name);

            //Cleanup
            await ResetDatabaseAsync();
        }
    }

    [Fact]
    public async Task Update_SendedIdEqualsZero_ValidationError()
    {
        using (TestServer = HostConfiguration.Build().Server)
        {
            //Initialisation
            await InitializeDatabaseAsync();

            //Arrange
            using HttpClient httpClient = TestServer.CreateClient();
            var command = _fixture.Build<UpdateNotificationTypeCommand>().With(x => x.Id, 0).Create();

            //Act
            HttpResponseMessage httpResponse = await httpClient.PutAsync($"/api/notification-types",
                    new StringContent(JsonConvert.SerializeObject(command, new StringEnumConverter()), Encoding.UTF8, MediaTypeNames.Application.Json));

            //Assess
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await httpResponse.Content.ReadAsStringAsync();
            var validationError = JsonConvert.DeserializeObject<ValidationError>(content);

            validationError.Should().NotBeNull();
            validationError!.Message.Should().NotBeNullOrEmpty();

            //Clean up
            await ResetDatabaseAsync();
        }
    }

    [Fact]
    public async Task GetAsync_SearchStringIsEmpty_AllPossibleEntities()
    {
        using (TestServer = HostConfiguration.Build().Server)
        {
            //Initialisation
            await InitializeDatabaseAsync();

            //Arrange
            using HttpClient httpClient = TestServer.CreateClient();
            using IServiceScope scope = TestServer.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>()!;

            var entities = _fixture.Build<NotificationType>().Without(x => x.NotificationTemplates).Without(x => x.Id).CreateMany(RandomHelper.Next(5, 10));

            await context.NotificationTypes.AddRangeAsync(entities, default);
            await context.SaveChangesAsync();

            var count = await context.NotificationTypes.CountAsync();

            //Act
            var query = _fixture.Build<GetNotificationTypesQuery>().With(x => x.SearchString, string.Empty).Create();

            var url = WebSerializer.ToQueryString("/api/notification-types", query);

            HttpResponseMessage httpResponse = await httpClient.GetAsync(url);

            //Assess
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await httpResponse.Content.ReadAsStringAsync();
            var retrivedEntity = JsonConvert.DeserializeObject<IEnumerable<NotificationTypeDto>>(content);

            retrivedEntity.Should().NotBeNull();
            retrivedEntity.Should().HaveCount(count);
            retrivedEntity!.All(x => entities.Any(z => z.Id == x.Id && z.Name == x.Name)).Should().BeTrue();

            //Clean up
            await ResetDatabaseAsync();
        }
    }

    [Fact]
    public async Task GetAsync_SearchStringDoesntMatchAnyEntity_EmptyArray()
    {
        using (TestServer = HostConfiguration.Build().Server)
        {
            //Initialisation
            await InitializeDatabaseAsync();

            //Arrange
            using HttpClient httpClient = TestServer.CreateClient();
            using IServiceScope scope = TestServer.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>()!;

            var entities = _fixture.Build<NotificationType>().Without(x => x.NotificationTemplates).Without(x => x.Id).CreateMany(RandomHelper.Next(5, 10));

            await context.NotificationTypes.AddRangeAsync(entities, default);
            await context.SaveChangesAsync();

            var count = await context.NotificationTypes.CountAsync();

            var query = _fixture.Create<GetNotificationTypesQuery>();
            var url = WebSerializer.ToQueryString("/api/notification-types", query);

            //Act
            HttpResponseMessage httpResponse = await httpClient.GetAsync(url);

            //Assess
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await httpResponse.Content.ReadAsStringAsync();
            var retrivedEntity = JsonConvert.DeserializeObject<IEnumerable<NotificationTypeDto>>(content);

            retrivedEntity.Should().NotBeNull();
            retrivedEntity.Should().HaveCountLessThan(count);
            retrivedEntity.Should().BeEmpty();

            await ResetDatabaseAsync();
        }
    }

    [Fact]
    public async Task GetAsync_SearchStringMatchesSomeEntities_AllEntitiesThanMatchSearchString()
    {
        using (TestServer = HostConfiguration.Build().Server)
        {
            //Initialisation
            await InitializeDatabaseAsync();

            //Arrange
            using HttpClient httpClient = TestServer.CreateClient();
            using IServiceScope scope = TestServer.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>()!;

            var searchString = "Dummy";

            var entities = _fixture.Build<NotificationType>().Without(x => x.NotificationTemplates).Without(x => x.Id).CreateMany(RandomHelper.Next(5, 10)).ToList();
            var matchingEntities = _fixture.Build<NotificationType>()
                .Without(x => x.NotificationTemplates).Without(x => x.Id)
                .With(x => x.Name, searchString + _fixture.Create<string>())
                .CreateMany(RandomHelper.Next(5, 10)).ToList();

            await context.NotificationTypes.AddRangeAsync(entities, default);
            await context.NotificationTypes.AddRangeAsync(matchingEntities, default);
            await context.SaveChangesAsync();

            var count = await context.NotificationTypes.CountAsync();

            var query = _fixture.Build<GetNotificationTypesQuery>().With(x => x.SearchString, searchString).Create();
            var url = WebSerializer.ToQueryString("/api/notification-types", query);

            //Act
            HttpResponseMessage httpResponse = await httpClient.GetAsync(url);

            //Assess
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await httpResponse.Content.ReadAsStringAsync();
            var retrivedEntity = JsonConvert.DeserializeObject<IEnumerable<NotificationTypeDto>>(content);

            retrivedEntity.Should().NotBeNull();
            retrivedEntity.Should().HaveCount(matchingEntities.Count);
            retrivedEntity!.All(x => matchingEntities.Any(z => z.Id == x.Id && z.Name == x.Name)).Should().BeTrue();

            await ResetDatabaseAsync();
        }
    }

    [Fact]
    public async Task GetAsync_GivenIdEqualsZero_ValidationError()
    {
        using (TestServer = HostConfiguration.Build().Server)
        {
            //Initialisation
            await InitializeDatabaseAsync();

            //Arrange
            using HttpClient httpClient = TestServer.CreateClient();

            //Act
            HttpResponseMessage httpResponse = await httpClient.GetAsync($"/api/notification-types/{0}");

            //Assess
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await httpResponse.Content.ReadAsStringAsync();

            var validationError = JsonConvert.DeserializeObject<ValidationError>(content);

            validationError.Should().NotBeNull();
            validationError!.Message.Should().NotBeNullOrEmpty();

            await ResetDatabaseAsync();
        }
    }

    [Fact]
    public async Task GetAsync_EntityWithGivenIdDoestExist_NotFound()
    {
        using (TestServer = HostConfiguration.Build().Server)
        {
            //Initialisation
            await InitializeDatabaseAsync();

            //Arrange
            using HttpClient httpClient = TestServer.CreateClient();

            var id = _fixture.Create<int>();

            //Act
            HttpResponseMessage httpResponse = await httpClient.GetAsync($"/api/notification-types/{id}");

            //Assess
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var content = await httpResponse.Content.ReadAsStringAsync();
            var notFound = JsonConvert.DeserializeObject<NotFoundError<int>>(content);

            notFound.Should().NotBeNull();
            notFound!.Message.Should().NotBeNullOrEmpty();

            await ResetDatabaseAsync();
        }
    }

    [Fact]
    public async Task GetAsync_EntityExists_EntityFetchAndReturned()
    {
        using (TestServer = HostConfiguration.Build().Server)
        {
            //Initialisation
            await InitializeDatabaseAsync();

            //Arrange
            using HttpClient httpClient = TestServer.CreateClient();
            using IServiceScope scope = TestServer.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>()!;

            var entities = _fixture.Build<NotificationType>().Without(x => x.NotificationTemplates).Without(x => x.Id).CreateMany().ToList();

            await context.NotificationTypes.AddRangeAsync(entities, default);
            await context.SaveChangesAsync();

            var randomEntity = entities[RandomHelper.Next(entities.Count - 1)];

            //Act
            HttpResponseMessage httpResponse = await httpClient.GetAsync($"/api/notification-types/{randomEntity.Id}");

            //Assess
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await httpResponse.Content.ReadAsStringAsync();
            var retrivedEntity = JsonConvert.DeserializeObject<NotificationTypeDto>(content);

            retrivedEntity.Should().NotBeNull();
            retrivedEntity!.Id.Should().Be(randomEntity.Id);
            retrivedEntity!.Name.Should().Be(randomEntity.Name);

            await ResetDatabaseAsync();
        }
    }
}
