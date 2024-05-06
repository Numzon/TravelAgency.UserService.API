using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using TravelAgency.UserService.API.IntegrationTests;
using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.NotificationTypes.Models;
using TravelAgency.UserService.Application.NotificationTypes.Queries.GetNotificationTypeQuery;
using TravelAgency.UserService.Application.NotificationTypes.Queries.GetNotificationTypesQuery;
using TravelAgency.UserService.Domain.Entities;
using TravelAgency.UserService.SharedTestLibrary.Helpers;
using TravelAgency.UserService.Tests.Shared.Helpers;

namespace TravelAgency.UserService.Application.IntegrationTests.NotificationTypes.Queries;

public sealed class GetNotificationTypeTests : BaseSqlServerDbTest<NotificationType>
{
    public GetNotificationTypeTests() : base()
    {
    }



    [Fact]
    public async Task GetAsync_SearchStringIsEmpty_AllPossibleEntities()
    {
        //Arrange
        var entities = _fixture.Build<NotificationType>().Without(x => x.NotificationTemplates).Without(x => x.Id).CreateMany(RandomHelper.Next(5, 10));

        entities = await CreateRangeAsync(entities);
        var count = await CountAsync();

        //Act
        var response = await SendAsync(new GetNotificationTypesQuery());

        //Assess
        response.Should().NotBeNull();
        response.Value.Should().NotBeNull();
        response.Error.Should().BeNull();
        response.IsSuccess.Should().BeTrue();

        var result = response.GetResult() as Ok<object>;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var retrivedEntity = result.Value as IEnumerable<NotificationTypeDto>;

        retrivedEntity.Should().NotBeNull();
        retrivedEntity.Should().HaveCount(count);
        retrivedEntity!.All(x => entities.Any(z => z.Id == x.Id && z.Name == x.Name)).Should().BeTrue();
    }

    [Fact]
    public async Task GetAsync_SearchStringDoesntMatchAnyEntity_EmptyArray()
    {
        //Arrange
        var entities = _fixture.Build<NotificationType>().Without(x => x.NotificationTemplates).Without(x => x.Id).CreateMany(RandomHelper.Next(5, 10));
        var count = entities.Count();

        await CreateRangeAsync(entities);
        var query = _fixture.Create<GetNotificationTypesQuery>();

        //Act
        var response = await SendAsync(query);

        //Assess
        response.Should().NotBeNull();
        response.Value.Should().NotBeNull();
        response.Error.Should().BeNull();
        response.IsSuccess.Should().BeTrue();

        var result = response.GetResult() as Ok<object>;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var retrivedEntity = result.Value as IEnumerable<NotificationTypeDto>;

        retrivedEntity.Should().NotBeNull();
        retrivedEntity.Should().HaveCountLessThan(count);
        retrivedEntity.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAsync_SearchStringMatchesSomeEntities_AllEntitiesThanMatchSearchString()
    {
        //Arrange
        var searchString = "Dummy";

        var entities = _fixture.Build<NotificationType>().Without(x => x.NotificationTemplates).Without(x => x.Id).CreateMany(RandomHelper.Next(5, 10)).ToList();
        var matchingEntities = _fixture.Build<NotificationType>()
            .Without(x => x.NotificationTemplates).Without(x => x.Id)
            .With(x => x.Name, searchString + _fixture.Create<string>())
            .CreateMany(RandomHelper.Next(5, 10)).ToList();

        await CreateRangeAsync(entities);
        await CreateRangeAsync(matchingEntities);

        var query = _fixture.Build<GetNotificationTypesQuery>().With(x => x.SearchString, searchString).Create();

        //Act
        var response = await SendAsync(query);

        //Assess
        response.Should().NotBeNull();
        response.Value.Should().NotBeNull();
        response.Error.Should().BeNull();
        response.IsSuccess.Should().BeTrue();

        var result = response.GetResult() as Ok<object>;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var retrivedEntity = result.Value as IEnumerable<NotificationTypeDto>;

        retrivedEntity.Should().NotBeNull();
        retrivedEntity.Should().HaveCount(matchingEntities.Count);
        retrivedEntity!.All(x => matchingEntities.Any(z => z.Id == x.Id && z.Name == x.Name)).Should().BeTrue();
    }
}
