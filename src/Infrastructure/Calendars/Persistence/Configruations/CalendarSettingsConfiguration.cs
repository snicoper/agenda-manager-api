using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Calendars.Persistence.Configruations;

public class CalendarSettingsConfiguration : IEntityTypeConfiguration<CalendarSettings>
{
    public void Configure(EntityTypeBuilder<CalendarSettings> builder)
    {
        builder.ToTable("CalendarSettings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => CalendarSettingsId.From(value))
            .IsRequired();

        builder.Property(x => x.CalendarId)
            .HasConversion(
                calendarId => calendarId.Value,
                value => CalendarId.From(value))
            .IsRequired();

        builder.HasOne(x => x.Calendar)
            .WithOne(c => c.Settings)
            .HasForeignKey<CalendarSettings>(x => x.CalendarId);

        builder.Property(x => x.TimeZone)
            .HasConversion(
                ianaTimezone => ianaTimezone.Value,
                value => IanaTimeZone.FromIana(value))
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.ConfirmationRequirement)
            .IsRequired();

        builder.Property(x => x.OverlapBehavior)
            .IsRequired();

        builder.Property(x => x.HolidayAppointmentHandling)
            .IsRequired();

        builder.Property(x => x.ScheduleValidation)
            .IsRequired();
    }
}
