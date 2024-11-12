using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Calendars.Persistence;

public class CalendarSettingsConfiguration : IEntityTypeConfiguration<CalendarSettings>
{
    public void Configure(EntityTypeBuilder<CalendarSettings> builder)
    {
        builder.ToTable("CalendarSettings");

        builder.HasKey(cs => cs.Id);

        builder.Property(cs => cs.Id)
            .HasConversion(
                id => id.Value,
                value => CalendarSettingsId.From(value))
            .IsRequired();

        builder.Property(cs => cs.CalendarId)
            .HasConversion(
                id => id.Value,
                value => CalendarId.From(value))
            .IsRequired();

        builder.HasOne(cs => cs.Calendar)
            .WithOne(c => c.Settings)
            .HasForeignKey<CalendarSettings>(cs => cs.CalendarId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(cs => cs.IanaTimeZone)
            .HasConversion(
                ianaTimeZone => ianaTimeZone.Value,
                value => IanaTimeZone.FromIana(value))
            .IsRequired();

        builder.Property(x => x.HolidayCreationStrategy)
            .IsRequired();
    }
}
