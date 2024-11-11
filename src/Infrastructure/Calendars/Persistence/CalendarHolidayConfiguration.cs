using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Calendars.Persistence;

public class CalendarHolidayConfiguration : IEntityTypeConfiguration<CalendarHoliday>
{
    public void Configure(EntityTypeBuilder<CalendarHoliday> builder)
    {
        builder.ToTable("CalendarHolidays");

        builder.HasKey(x => x.Id);

        builder.Property(ch => ch.Id)
            .HasConversion(
                id => id.Value,
                value => CalendarHolidayId.From(value))
            .IsRequired();

        builder.Property(ch => ch.CalendarId)
            .HasConversion(
                calendarId => calendarId.Value,
                value => CalendarId.From(value))
            .IsRequired();

        builder.HasOne(ch => ch.Calendar)
            .WithMany(c => c.Holidays)
            .HasForeignKey(ch => ch.CalendarId);

        builder.OwnsOne(
            calendarHoliday => calendarHoliday.Period,
            calendarHolidayBuilder =>
            {
                calendarHolidayBuilder.Property(p => p.StartDate)
                    .HasColumnName("StartDate");

                calendarHolidayBuilder.Property(p => p.EndDate)
                    .HasColumnName("EndDate");
            });

        builder.Property(ch => ch.AvailableDays)
            .IsRequired();

        builder.Property(ch => ch.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ch => ch.Description)
            .HasMaxLength(500)
            .IsRequired();
    }
}
