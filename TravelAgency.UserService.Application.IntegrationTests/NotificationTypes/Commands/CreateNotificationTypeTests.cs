﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using TravelAgency.UserService.API.IntegrationTests;
using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.NotificationTypes.Commands.CreateNotificationType;
using TravelAgency.UserService.Application.NotificationTypes.Models;
using TravelAgency.UserService.Domain.Entities;

namespace TravelAgency.UserService.Application.IntegrationTests.NotificationTypes.Commands;


public sealed class CreateNotificationTypeTests : BaseSqlServerDbTest<NotificationType>
{
    public CreateNotificationTypeTests() : base()
    {
    }

    [Fact]
    public async Task Handle_ValidTypeName_CreatesAndReturnsNotificationType()
    {
        //Arrange
        var command = _fixture.Create<CreateNotificationTypeCommand>();

        //Act
        var response = await SendAsync(command);

        //Assert
        response.Should().NotBeNull();
        response.Value.Should().NotBeNull();
        response.Error.Should().BeNull();
        response.IsSuccess.Should().BeTrue();

        var createdAtRoute = response.GetResult() as CreatedAtRoute<object>;

        createdAtRoute.Should().NotBeNull();    

        var typeDto = createdAtRoute!.Value as NotificationTypeDto;

        typeDto.Should().NotBeNull();   

        var retrivedType = await FirstOrDefaultAsync(x => x.Id == typeDto!.Id);

        retrivedType.Should().NotBeNull();
        retrivedType!.Id.Should().NotBe(0);
        retrivedType!.Name.Should().Be(command.Name);
    }

    [Fact]
    public async Task Handle_InvalidName_ErrorNoActionCompleted()
    {
        //Arrange
        var command = new CreateNotificationTypeCommand(null!);

        //Act
        var response = await SendAsync(command);

        //Assert
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
    public async Task Handle_SendedNameIsNullOrEmpty_ValidationError()
    {
        //Arrange
        var command = _fixture.Build<CreateNotificationTypeCommand>().With(x => x.Name, string.Empty).Create();

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
