using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.UserService.Domain.Entities;

namespace TravelAgency.UserService.Infrastructure.Persistance.Configuration;
public class NotificationTemplateConfiguration : IEntityTypeConfiguration<NotificationTemplate>
{
    public void Configure(EntityTypeBuilder<NotificationTemplate> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Description)
            .IsRequired();

        builder
            .HasOne(x => x.NotificationType)
            .WithMany(x => x.NotificationTemplates)
            .HasForeignKey(x => x.NotificationTypeId)
            .IsRequired();
    }
}
