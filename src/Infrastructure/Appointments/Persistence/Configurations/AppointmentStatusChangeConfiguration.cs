using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Appointments.Persistence.Configurations;

public class AppointmentStatusChangeConfiguration : IEntityTypeConfiguration<AppointmentStatusChange>
{
    public void Configure(EntityTypeBuilder<AppointmentStatusChange> builder)
    {
        builder.ToTable("AppointmentStatusChanges");

        builder.HasKey(asc => asc.Id);

        builder
            .Property(asc => asc.Id)
            .HasConversion(
                id => id.Value,
                value => AppointmentStatusChangeId.From(value))
            .IsRequired();

        builder
            .Property(asc => asc.AppointmentId)
            .HasConversion(
                appointmentId => appointmentId.Value,
                value => AppointmentId.From(value))
            .IsRequired();

        builder.HasOne(asc => asc.Appointment)
            .WithMany(a => a.StatusChanges)
            .HasForeignKey(asc => asc.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(
            appointment => appointment.Period,
            appointmentBuilder =>
            {
                appointmentBuilder.Property(p => p.StartDate)
                    .HasColumnName("StartDate");

                appointmentBuilder.Property(p => p.EndDate)
                    .HasColumnName("EndDate");
            });

        builder.Property(asc => asc.Status)
            .IsRequired();

        builder.Property(adc => adc.IsCurrentStatus);

        builder.Property(asc => asc.Duration)
            .IsRequired();

        builder.Property(asc => asc.Description)
            .HasMaxLength(200);
    }
}
