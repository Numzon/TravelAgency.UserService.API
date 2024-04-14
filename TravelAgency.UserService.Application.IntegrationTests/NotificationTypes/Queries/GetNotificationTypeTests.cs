using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using TravelAgency.UserService.API.IntegrationTests;
using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.NotificationTypes.Models;
using TravelAgency.UserService.Application.NotificationTypes.Queries.GetNotificationTypeQuery;
using TravelAgency.UserService.Domain.Entities;
using TravelAgency.UserService.SharedTestLibrary.Helpers;

namespace TravelAgency.UserService.Application.IntegrationTests.NotificationTypes.Queries;

public sealed class GetNotificationTypeTests : BaseSqlServerDbTest<NotificationType>
{
    public GetNotificationTypeTests() : base()
    {
    }

    [Fact]
    public async Task Handle_GivenIdEqualsZero_ValidationError()
    {
        //Arrange
        var command = _fixture.Build<GetNotificationTypeQuery>().With(x => x.Id, 0).Create();

        //Act
        var response = await SendAsync(command);

        //Assess
        response.Should().NotBeNull();
        response.Error.Should().NotBeNull();
        response.Value.Should().BeNull();
        response.IsSuccess.Should().BeFalse();

        var validationError = response.GetResult() as BadRequest<ValidationError>;

        validationError.Should().NotBeNull();
        validationError!.Value.Should().NotBeNull();
        validationError!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        validationError!.Value!.Message.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Handle_EntityWithGivenIdDoestExist_NotFound()
    {
        //Arrange
        var id = _fixture.Create<int>();

        //Act
        var response = await SendAsync(new GetNotificationTypeQuery(id));

        //Assess
        response.Should().NotBeNull();
        response.Error.Should().NotBeNull();
        response.Value.Should().BeNull();
        response.IsSuccess.Should().BeFalse();

        var notFound = response.GetResult() as NotFound<NotFoundError>;

        notFound.Should().NotBeNull();
        notFound!.Value.Should().NotBeNull();
        notFound!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        notFound!.Value!.Message.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Handle_EntityExists_EntityFetchAndReturned()
    {
        //Arrange
        var entities = _fixture.Build<NotificationType>().Without(x => x.NotificationTemplates).Without(x => x.Id).CreateMany().ToList();

        entities = (await CreateRangeAsync(entities)).ToList();
        var randomEntity = entities[RandomHelper.Next(entities.Count - 1)];

        //Act
        var response = await SendAsync(new GetNotificationTypeQuery(randomEntity.Id));

        //Assess
        response.Should().NotBeNull();
        response.Value.Should().NotBeNull();
        response.Error.Should().BeNull();
        response.IsSuccess.Should().BeTrue();

        var result = response.GetResult() as Ok<object>;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var retrivedEntity = result.Value as NotificationTypeDto;

        retrivedEntity.Should().NotBeNull();
        retrivedEntity!.Id.Should().Be(randomEntity.Id);
        retrivedEntity!.Name.Should().Be(randomEntity.Name);
    }
}
