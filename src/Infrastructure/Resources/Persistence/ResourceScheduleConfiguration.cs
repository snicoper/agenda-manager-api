﻿using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Resources.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Resources.Persistence;

public class ResourceScheduleConfiguration : IEntityTypeConfiguration<ResourceSchedule>
{
    public void Configure(EntityTypeBuilder<ResourceSchedule> builder)
    {
        builder.ToTable("ResourceSchedules");

        builder.HasKey(rs => rs.Id);

        builder.Property(rs => rs.Id)
            .HasConversion(
                id => id.Value,
                value => ResourceScheduleId.From(value))
            .IsRequired();

        builder.Property(rs => rs.ResourceId)
            .HasConversion(
                resourceId => resourceId.Value,
                value => ResourceId.From(value))
            .IsRequired();

        builder.HasOne(rs => rs.Resource)
            .WithMany()
            .HasForeignKey(rs => rs.ResourceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(rs => rs.CalendarId)
            .HasConversion(
                calendarId => calendarId.Value,
                value => CalendarId.From(value))
            .IsRequired();

        builder.HasOne(rs => rs.Calendar)
            .WithMany()
            .HasForeignKey(rs => rs.CalendarId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(
            rs => rs.Period,
            resourceScheduleBuilder =>
            {
                resourceScheduleBuilder.Property(p => p.StartDate)
                    .HasColumnName("StartDate");

                resourceScheduleBuilder.Property(p => p.EndDate)
                    .HasColumnName("EndDate");
            });

        builder.Property(rs => rs.Type)
            .IsRequired();

        builder.Property(rs => rs.AvailableDays)
            .IsRequired();

        builder.Property(rs => rs.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(rs => rs.Description)
            .HasMaxLength(500)
            .IsRequired();
    }
}
