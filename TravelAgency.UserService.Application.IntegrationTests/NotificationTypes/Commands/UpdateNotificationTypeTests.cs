using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using TravelAgency.UserService.API.IntegrationTests;
using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.NotificationTypes.Commands.UpdateNotificationType;
using TravelAgency.UserService.Domain.Entities;

namespace TravelAgency.UserService.Application.IntegrationTests.NotificationTypes.Commands;


public sealed class UpdateNotificationTypeTests : BaseSqlServerDbTest<NotificationType>
{
    public UpdateNotificationTypeTests() : base()
    {
    }

    [Fact]
    public async Task Handle_EntityDoesntExist_NotFoundError()
    {
        //Arrange
        var command = _fixture.Create<UpdateNotificationTypeCommand>();

        //Act
        var response = await SendAsync(command);

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
    public async Task Handle_EntityExists_EntityUpdated()
    {
        //Arrange
        var entity = _fixture.Build<NotificationType>().Without(x => x.NotificationTemplates).Without(x => x.Id).Create();
        var newName = _fixture.Create<string>();

        entity = await CreateAsync(entity);
        var count = await CountAsync();

        //Act
        var response = await SendAsync(new UpdateNotificationTypeCommand(entity.Id, newName));

        //Assess
        response.Should().NotBeNull();
        response.Value.Should().NotBeNull();
        response.Error.Should().BeNull();
        response.IsSuccess.Should().BeTrue();

        var result = response.GetResult() as NoContent;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status204NoContent);

        var currentCount = await CountAsync();

        currentCount.Should().Be(count);

        var retrivedEntity = await FirstOrDefaultAsync(x => x.Id == entity.Id);

        retrivedEntity.Should().NotBeNull();
        retrivedEntity!.Name.Should().NotBe(entity.Name);
        retrivedEntity!.Name.Should().Be(newName);
    }

    [Fact]
    public async Task Handle_SendedIdEqualsZero_ValidationError()
    {
        //Arrange
        var command = _fixture.Build<UpdateNotificationTypeCommand>().With(x => x.Id, 0).Create();

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
}
