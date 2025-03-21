﻿using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Resources;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.ResourceManagement.Resources.Persistence.Configruations;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.ToTable("Resources");

        builder.HasKey(c => c.Id);

        builder.HasIndex(r => new { r.CalendarId, r.Name })
            .IsUnique();

        builder.Property(r => r.Id)
            .HasConversion(
                id => id.Value,
                value => ResourceId.From(value))
            .IsRequired();

        builder.Property(r => r.UserId)
            .HasConversion(
                userId => userId != null ? userId.Value : (Guid?)null,
                value => value != null ? UserId.From(value.Value) : null);

        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(r => r.CalendarId)
            .HasConversion(
                calendarId => calendarId.Value,
                value => CalendarId.From(value))
            .IsRequired();

        builder.HasOne(r => r.Calendar)
            .WithMany()
            .HasForeignKey(r => r.CalendarId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(r => r.TypeId)
            .HasConversion(
                typeId => typeId.Value,
                value => ResourceTypeId.From(value))
            .IsRequired();

        builder.HasOne(r => r.Type)
            .WithMany(rt => rt.Resources)
            .HasForeignKey(r => r.TypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(r => r.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.OwnsOne(
            r => r.ColorScheme,
            colorSchemeBuilder =>
            {
                colorSchemeBuilder.Property(cs => cs.Text)
                    .HasColumnName("TextColor")
                    .HasMaxLength(7)
                    .IsRequired();

                colorSchemeBuilder.Property(cs => cs.Background)
                    .HasColumnName("BackgroundColor")
                    .HasMaxLength(7)
                    .IsRequired();
            });

        builder.Property(r => r.IsActive);

        builder.Property(r => r.DeactivationReason)
            .HasMaxLength(256);

        builder.HasMany(r => r.Schedules)
            .WithOne(s => s.Resource)
            .HasForeignKey(s => s.ResourceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
