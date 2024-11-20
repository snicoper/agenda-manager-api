using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.ValueObjects.Duration;
using AgendaManager.Domain.ResourceTypes.ValueObjects;
using AgendaManager.Domain.Services;
using AgendaManager.Domain.Services.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Services.Persistence;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => ServiceId.From(value))
            .IsRequired();

        builder.Property(s => s.CalendarId)
            .HasConversion(
                id => id.Value,
                value => CalendarId.From(value))
            .IsRequired();

        builder.HasOne(s => s.Calendar)
            .WithMany()
            .HasForeignKey(s => s.CalendarId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(s => s.Duration)
            .HasConversion(
                duration => duration.Value,
                value => Duration.From(value))
            .IsRequired();

        builder.Property(s => s.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.OwnsOne(
            s => s.ColorScheme,
            serviceBuilder =>
            {
                serviceBuilder.Property(p => p.Text)
                    .HasColumnName("TextColor");

                serviceBuilder.Property(p => p.Background)
                    .HasColumnName("BackgroundColor");
            });

        builder.Property(s => s.IsActive);

        builder.HasMany(s => s.ResourceTypes)
            .WithMany(rt => rt.Services)
            .UsingEntity(
                typeBuilder =>
                {
                    const string serviceIdName = nameof(ServiceId);
                    const string resourceTypeIdName = nameof(ResourceTypeId);

                    typeBuilder.ToTable("ServiceResourceTypes");
                    typeBuilder.Property<ServiceId>(serviceIdName).HasColumnName(serviceIdName);
                    typeBuilder.Property<ResourceTypeId>(resourceTypeIdName).HasColumnName(resourceTypeIdName);
                    typeBuilder.HasKey(serviceIdName, resourceTypeIdName);

                    // Campos de auditoría.
                    typeBuilder.Property<DateTimeOffset>(nameof(AuditableEntity.CreatedAt)).IsRequired();
                    typeBuilder.Property<string>(nameof(AuditableEntity.CreatedBy)).IsRequired();
                    typeBuilder.Property<DateTimeOffset>(nameof(AuditableEntity.LastModifiedAt)).IsRequired();
                    typeBuilder.Property<string>(nameof(AuditableEntity.LastModifiedBy)).IsRequired();
                });
    }
}
