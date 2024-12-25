using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Calendars.Persistence.Configruations;

public class CalendarConfiguration : IEntityTypeConfiguration<Calendar>
{
    public void Configure(EntityTypeBuilder<Calendar> builder)
    {
        builder.ToTable("Calendars");

        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.Name)
            .IsUnique();

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => CalendarId.From(value))
            .IsRequired();

        builder.Property(c => c.SettingsId)
            .HasConversion(
                settingsId => settingsId.Value,
                value => CalendarSettingsId.From(value))
            .IsRequired();

        builder.HasOne(c => c.Settings)
            .WithOne()
            .HasForeignKey<Calendar>(c => c.SettingsId);

        builder.Property(c => c.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(c => c.IsActive);

        builder.HasMany(c => c.Holidays)
            .WithOne(ch => ch.Calendar)
            .HasForeignKey(c => c.CalendarId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
