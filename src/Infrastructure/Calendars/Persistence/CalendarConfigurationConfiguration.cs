using AgendaManager.Domain.Calendars.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Calendars.Persistence;

public class
    CalendarConfigurationConfiguration : IEntityTypeConfiguration<Domain.Calendars.Entities.CalendarConfiguration>
{
    public void Configure(EntityTypeBuilder<Domain.Calendars.Entities.CalendarConfiguration> builder)
    {
        builder.ToTable("CalendarConfigurations");

        builder.HasKey(cc => cc.Id);

        builder.HasIndex(cc => new { cc.CalendarId, cc.Category });

        builder.Property(cc => cc.Id)
            .HasConversion(
                id => id.Value,
                value => CalendarConfigurationId.From(value))
            .IsRequired();

        builder.Property(cc => cc.CalendarId)
            .HasConversion(
                calendarId => calendarId.Value,
                value => CalendarId.From(value))
            .IsRequired();

        builder.HasOne(cc => cc.Calendar)
            .WithMany(calendar => calendar.Configurations)
            .HasForeignKey(cc => cc.CalendarId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(cc => cc.Category)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(cc => cc.SelectedKey)
            .HasMaxLength(100)
            .IsRequired();
    }
}
