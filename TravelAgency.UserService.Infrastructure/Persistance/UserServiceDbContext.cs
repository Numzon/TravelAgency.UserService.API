using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Domain.Entities;
using TravelAgency.UserService.Infrastructure.Extensions;
using TravelAgency.UserService.Infrastructure.Persistance.Interceptors;

namespace TravelAgency.UserService.Infrastructure.Persistance;
public class UserServiceDbContext : DbContext, IUserServiceDbContext
{
    private readonly IMediator _mediator;
    private readonly BaseAuditableEntitySaveChangesInterceptor _baseAuditableEntitySaveChangesInterceptor;

    public DbSet<NotificationTemplate> NotificationTemplate { get; set; }
    public DbSet<NotificationType> NotificationTypes { get; set; }

    public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options,
        IMediator mediator,
        BaseAuditableEntitySaveChangesInterceptor baseAuditableEntitySaveChangesInterceptor) : base(options)
    {
        _baseAuditableEntitySaveChangesInterceptor = baseAuditableEntitySaveChangesInterceptor;
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_baseAuditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}

