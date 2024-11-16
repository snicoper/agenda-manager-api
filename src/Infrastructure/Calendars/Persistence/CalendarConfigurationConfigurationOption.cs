using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Calendars.Persistence;

public class CalendarConfigurationConfigurationOption : IEntityTypeConfiguration<CalendarConfigurationOption>
{
    public void Configure(EntityTypeBuilder<CalendarConfigurationOption> builder)
    {
        builder.ToTable("CalendarConfigurationOptions");

        builder.HasKey(c => c.OptionId);

        builder.Property(c => c.OptionId)
            .HasConversion(
                id => id.Value,
                value => CalandarConfigurationOptionId.From(value))
            .IsRequired();

        builder.Property(c => c.Category)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Key)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(c => c.DefaultValue);
    }
}
