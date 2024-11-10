using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Services.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Appointments.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(a => a.Id);

        builder
            .Property(a => a.Id)
            .HasConversion(
                id => id.Value,
                value => AppointmentId.From(value))
            .IsRequired();

        builder
            .Property(a => a.ServiceId)
            .HasConversion(
                serviceId => serviceId.Value,
                value => ServiceId.From(value))
            .IsRequired();

        builder.HasOne(a => a.Service)
            .WithMany()
            .HasForeignKey(a => a.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(a => a.CalendarId)
            .HasConversion(
                calendarId => calendarId.Value,
                value => CalendarId.From(value))
            .IsRequired();

        builder.HasOne(a => a.Calendar)
            .WithMany()
            .HasForeignKey(a => a.CalendarId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(a => a.UserId)
            .HasConversion(
                userId => userId.Value,
                value => UserId.From(value))
            .IsRequired();

        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.StatusChanges)
            .WithOne(sc => sc.Appointment)
            .HasForeignKey(sc => sc.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(
            a => a.Period,
            appointmentBuilder =>
            {
                appointmentBuilder.Property(p => p.StartDate)
                    .HasColumnName("StartDate");

                appointmentBuilder.Property(p => p.EndDate)
                    .HasColumnName("EndDate");
            });

        builder.Property(a => a.Status)
            .IsRequired();
    }
}
