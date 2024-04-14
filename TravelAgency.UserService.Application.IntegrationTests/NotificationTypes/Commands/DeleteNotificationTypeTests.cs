using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using TravelAgency.UserService.API.IntegrationTests;
using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.NotificationTypes.Commands.DeleteNotificationType;
using TravelAgency.UserService.Application.NotificationTypes.Commands.UpdateNotificationType;
using TravelAgency.UserService.Domain.Entities;

namespace TravelAgency.UserService.Application.IntegrationTests.NotificationTypes.Commands;

using static BaseSqlServerDbTest<NotificationType>;

public sealed class DeleteNotificationTypeTests //: BaseSqlServerDbTest<NotificationType>
{
    private readonly Fixture _fixture;

    public DeleteNotificationTypeTests() : base()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task Handle_EntityWithGivenIdDoestExist_NotFound()
    {
        //Arrange
        await InitializeAsync();
        await ResetState();
        var id = _fixture.Create<int>();

        //Act
        var response = await SendAsync(new DeleteNotificationTypeCommand(id));

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
        await DisposeAsync();
    }


    [Fact]
    public async Task Handle_EntityExists_EntityDeleted()
    {
        //Arrange
        await InitializeAsync();
        await ResetState();
        var entity = _fixture.Build<NotificationType>().Without(x => x.NotificationTemplates).Without(x => x.Id).Create();

        entity = await CreateAsync(entity);
        var count = await CountAsync();

        //Act
        var response = await SendAsync(new DeleteNotificationTypeCommand(entity.Id));

        //Assess
        response.Should().NotBeNull();
        response.Value.Should().NotBeNull();
        response.Error.Should().BeNull();
        response.IsSuccess.Should().BeTrue();

        var result = response.GetResult() as NoContent;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status204NoContent);

        var currentCount = await CountAsync();

        currentCount.Should().BeLessThan(count);

        var retrivedEntity = await FirstOrDefaultAsync(x => x.Id == entity.Id);

        retrivedEntity.Should().BeNull();
        await DisposeAsync();
    }

    [Fact]
    public async Task Handle_SendedIdEqualsZero_ValidationError()
    {
        //Arrange
        await InitializeAsync();
        await ResetState();
        var command = _fixture.Build<DeleteNotificationTypeCommand>().With(x => x.Id, 0).Create();

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
        await DisposeAsync();
    }
}
