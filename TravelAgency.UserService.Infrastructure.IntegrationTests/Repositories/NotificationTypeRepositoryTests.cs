using Microsoft.EntityFrameworkCore;
using System.Data;
using TravelAgency.UserService.Application.NotificationTypes.Commands.CreateNotificationType;
using TravelAgency.UserService.Application.NotificationTypes.Queries.GetNotificationTypesQuery;
using TravelAgency.UserService.Domain.Entities;
using TravelAgency.UserService.Infrastructure.Repositories;
using TravelAgency.UserService.SharedTestLibrary.BaseTests;
using TravelAgency.UserService.Tests.Shared.Helpers;

namespace TravelAgency.UserService.Infrastructure.IntegrationTests.Repositories;
public sealed class NotificationTypeRepositoryTests : BaseInMemoryDbTest
{
    public NotificationTypeRepositoryTests() : base()
    {
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task CreateAsync_ValidName_AddsTypeToDb()
    {
        var command = _fixture.Create<CreateNotificationTypeCommand>();
        var notificationTypeRepository = new NotificationTypeRepository(_dbContext);

        var type = await notificationTypeRepository.CreateAsync(command, default);

        var retrievedType = _dbContext.NotificationTypes.FirstOrDefault(x => x.Id == type.Id);

        retrievedType.Should().NotBeNull();
        retrievedType!.Id.Should().Be(type.Id);
        retrievedType!.Name.Should().Be(type.Name);
        retrievedType.Created.Should().Be(type.Created);
        retrievedType.CreatedBy.Should().NotBeNullOrEmpty().And.Be(type.CreatedBy);
    }

    [Fact]
    public async Task CreateAsync_NameIsNull_ThrowsDbUpdateException()
    {
        var command = new CreateNotificationTypeCommand(null!);
        var notificationTypeRepository = new NotificationTypeRepository(_dbContext);

        await notificationTypeRepository.Invoking(x => x.CreateAsync(command, default)).Should().ThrowAsync<DbUpdateException>();
    }

    [Fact]
    public async Task CreateAsync_NameIsAnEmptySting_AddsTypeToDb()
    {
        var command = new CreateNotificationTypeCommand(string.Empty);
        var notificationTypeRepository = new NotificationTypeRepository(_dbContext);

        var type = await notificationTypeRepository.CreateAsync(command, default);
        var retrievedType = _dbContext.NotificationTypes.FirstOrDefault(x => x.Id == type.Id);

        retrievedType.Should().NotBeNull();
        retrievedType!.Id.Should().Be(type.Id);
        retrievedType!.Name.Should().Be(string.Empty);
        retrievedType.Created.Should().Be(type.Created);
        retrievedType.CreatedBy.Should().NotBeNullOrEmpty().And.Be(type.CreatedBy);
    }

    [Fact]
    public async Task GetByFitlersAsync_SearchStringIsEmpty_AllTypes()
    {
        var query = _fixture.Build<GetNotificationTypesQuery>().Without(x => x.SearchString).Create();
        var notificationTypeRepository = new NotificationTypeRepository(_dbContext);
        var randomCount = RandomHelper.Next(3, 10);
        var types = _fixture.Build<NotificationType>().Without(x => x.Id).CreateMany(randomCount).ToList();

        await _dbContext.AddRangeAsync(types, default);
        await _dbContext.SaveChangesAsync(default);

        var retrivedTypes = await notificationTypeRepository.GetByFitlersAsync(query, default);

        retrivedTypes.Should().NotBeNullOrEmpty();
        retrivedTypes.Should().HaveCount(randomCount);
    }

    [Fact]
    public async Task GetByFitlersAsync_SearchStringIsFilledButDoesntMatchAnyType_EmptyArray()
    {
        var query = _fixture.Build<GetNotificationTypesQuery>().With(x => x.SearchString, Guid.NewGuid().ToString()).Create();
        var notificationTypeRepository = new NotificationTypeRepository(_dbContext);
        var randomCount = RandomHelper.Next(3, 10);
        var types = _fixture.Build<NotificationType>().CreateMany(randomCount);

        await _dbContext.AddRangeAsync(types, default);
        await _dbContext.SaveChangesAsync(default);

        var retrivedTypes = await notificationTypeRepository.GetByFitlersAsync(query, default);

        retrivedTypes.Should().NotBeNull();
        retrivedTypes.Should().HaveCount(0);
    }

    [Fact]
    public async Task GetByFitlersAsync_SearchStringIsFilledAndMatchesSomeRecords_RecordsThatMatchesSearchString()
    {
        var keyWord = "KeyWord";
        var query = _fixture.Build<GetNotificationTypesQuery>().With(x => x.SearchString, keyWord).Create();
        var notificationTypeRepository = new NotificationTypeRepository(_dbContext);

        var typesWhoContainsCount = RandomHelper.Next(1, 5);
        var randomTypesCount = RandomHelper.Next(1, 5);
        var typesWhoContains = _fixture.Build<NotificationType>().With(x => x.Name, keyWord + Guid.NewGuid().ToString()).CreateMany(typesWhoContainsCount);
        var randomTypes = _fixture.Build<NotificationType>().CreateMany(randomTypesCount);

        await _dbContext.AddRangeAsync(typesWhoContains, default);
        await _dbContext.AddRangeAsync(randomTypes, default);
        await _dbContext.SaveChangesAsync(default);
        var count = await _dbContext.NotificationTypes.CountAsync(default);

        var retrivedTypes = await notificationTypeRepository.GetByFitlersAsync(query, default);

        retrivedTypes.Should().NotBeNull();
        retrivedTypes.Should().HaveCount(typesWhoContainsCount);
        retrivedTypes.All(x => x.Name.Contains(keyWord)).Should().BeTrue();
        count.Should().BeGreaterThan(typesWhoContainsCount);
    }

    [Fact]
    public async Task GetByIdAsync_IdIsZero_Null()
    {
        var id = 0;
        var randomTypes = _fixture.Build<NotificationType>().CreateMany(RandomHelper.Next(1, 5));
        var notificationTypeRepository = new NotificationTypeRepository(_dbContext);

        await _dbContext.AddRangeAsync(randomTypes, default);
        await _dbContext.SaveChangesAsync(default);

        var type = await notificationTypeRepository.GetByIdAsync(id, default);

        randomTypes.Should().NotBeEmpty();
        type.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_IdDoesntExist_Null()
    {
        var randomTypes = _fixture.Build<NotificationType>().CreateMany(RandomHelper.Next(1, 5));
        var id = randomTypes.Select(x => x.Id).Max() + RandomHelper.Next(1, 10);
        var notificationTypeRepository = new NotificationTypeRepository(_dbContext);

        await _dbContext.AddRangeAsync(randomTypes, default);
        await _dbContext.SaveChangesAsync(default);

        var type = await notificationTypeRepository.GetByIdAsync(id, default);

        randomTypes.Should().NotBeEmpty();
        type.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_IdExistsInDb_ValidType()
    {
        var maxValue = 20;
        var count = RandomHelper.Next(10, maxValue);
        var randomTypes = _fixture.Build<NotificationType>().CreateMany(count).ToList();
        var id = randomTypes[RandomHelper.Next(maxValue - 1)].Id;
        var notificationTypeRepository = new NotificationTypeRepository(_dbContext);

        await _dbContext.AddRangeAsync(randomTypes, default);
        await _dbContext.SaveChangesAsync(default);

        var type = await notificationTypeRepository.GetByIdAsync(id, default);

        randomTypes.Should().NotBeEmpty();
        type.Should().NotBeNull();
        type!.Id.Should().Be(id);
    }

    [Fact]
    public async Task UpdateAsync_TypeDoesntExist_NothingChanged()
    {
        var randomTypes = _fixture.Build<NotificationType>().With(x => x.Id, 0).CreateMany(RandomHelper.Next(5, 10)).ToList();
        var type = randomTypes.First();
        randomTypes.Remove(type);
        var repository = new NotificationTypeRepository(_dbContext);

        await _dbContext.AddRangeAsync(randomTypes, default);
        await _dbContext.SaveChangesAsync(default);

        await repository.UpdateAsync(type, default);

        var dbTypes = _dbContext.NotificationTypes.ToList();
        dbTypes.Should().NotBeNull();
        dbTypes.Should().HaveCount(randomTypes.Count);
        dbTypes.FirstOrDefault(x => x.Id == type.Id).Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_SlightlyModifiedTypeThatExistsInDatabase_TypeHasBeenUpdated()
    {
        var newName = "NEW NAME";
        var randomTypes = _fixture.Build<NotificationType>().With(x => x.Id, 0).CreateMany(RandomHelper.Next(5, 10)).ToList();
        var repository = new NotificationTypeRepository(_dbContext);

        await _dbContext.AddRangeAsync(randomTypes, default);
        await _dbContext.SaveChangesAsync(default);

        var type = randomTypes.First();
        type.Name = newName;

        await repository.UpdateAsync(type, default);

        var dbType = await _dbContext.NotificationTypes.FirstOrDefaultAsync(x => x.Id == type.Id);

        dbType.Should().NotBeNull();
        dbType!.Name.Should().Be(newName);
    }


    [Fact]
    public async Task UpdateAsync_SendedTypeIsNull_ThrowsArgumentNullException()
    {
        var randomTypes = _fixture.Build<NotificationType>().With(x => x.Id, 0).CreateMany(RandomHelper.Next(5, 10)).ToList();
        var repository = new NotificationTypeRepository(_dbContext);

        await _dbContext.AddRangeAsync(randomTypes, default);
        await _dbContext.SaveChangesAsync(default);

        await repository.Invoking(x => x.UpdateAsync(null!, default)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteAsync_TypeDoesntExist_ThrowsDbUpdateConcurrencyException()
    {
        var randomTypes = _fixture.Build<NotificationType>().With(x => x.Id, 0).CreateMany(RandomHelper.Next(5, 10)).ToList();
        var type = randomTypes.First();
        randomTypes.Remove(type);
        var repository = new NotificationTypeRepository(_dbContext);

        await _dbContext.AddRangeAsync(randomTypes, default);
        await _dbContext.SaveChangesAsync(default);

        await repository.Invoking(x => x.DeleteAsync(type, default)).Should().ThrowAsync<DbUpdateConcurrencyException>();
    }

    [Fact]
    public async Task DeleteAsync_TypeExistsInDb_TypeHasBeenDeleted()
    {
        var randomTypes = _fixture.Build<NotificationType>().With(x => x.Id, 0).CreateMany(RandomHelper.Next(5, 10)).ToList();
        var repository = new NotificationTypeRepository(_dbContext);

        await _dbContext.AddRangeAsync(randomTypes, default);
        await _dbContext.SaveChangesAsync(default);

        var type = randomTypes.First();

        await repository.DeleteAsync(type, default);

        var dbType = await _dbContext.NotificationTypes.FirstOrDefaultAsync(x => x.Id == type.Id);
        var count = await _dbContext.NotificationTypes.CountAsync(default);

        dbType.Should().BeNull();
        randomTypes.Should().HaveCountGreaterThan(count);
    }


    [Fact]
    public async Task DeleteAsync_SendedTypeIsNull_Throws()
    {
        var randomTypes = _fixture.Build<NotificationType>().CreateMany(RandomHelper.Next(5, 10)).ToList();
        var repository = new NotificationTypeRepository(_dbContext);

        await _dbContext.AddRangeAsync(randomTypes, default);
        await _dbContext.SaveChangesAsync(default);

        await repository.Invoking(x => x.DeleteAsync(null!, default)).Should().ThrowAsync<ArgumentNullException>();
    }
}
