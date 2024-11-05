using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Resources.ValueObjects;
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
                serviceBuilder.Property(p => p.TextColor)
                    .HasColumnName("TextColor");

                serviceBuilder.Property(p => p.BackgroundColor)
                    .HasColumnName("BackgroundColor");
            });

        builder.HasMany(s => s.ResourceTypes)
            .WithMany(rt => rt.Services)
            .UsingEntity(
                typeBuilder =>
                {
                    typeBuilder.ToTable("ServiceResourceTypes");
                    typeBuilder.Property<ServiceId>("ServiceId").HasColumnName("ServiceId");
                    typeBuilder.Property<ResourceTypeId>("ResourceTypeId").HasColumnName("ResourceTypeId");
                    typeBuilder.HasKey("ServiceId", "ResourceTypeId");

                    // Campos de auditoría.
                    typeBuilder.Property<DateTimeOffset>("CreatedAt");
                    typeBuilder.Property<string>("CreatedBy");
                    typeBuilder.Property<DateTimeOffset>("LastModifiedAt");
                    typeBuilder.Property<string>("LastModifiedBy");
                });
    }
}
