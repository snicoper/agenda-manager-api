using AgendaManager.Domain.Appointments.Entities;
using AgendaManager.Domain.Appointments.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Appointments.Persistence.Configurations;

public class AppointmentStatusChangeConfiguration : IEntityTypeConfiguration<AppointmentStatusHistory>
{
    public void Configure(EntityTypeBuilder<AppointmentStatusHistory> builder)
    {
        builder.ToTable("AppointmentStatusHistories");

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
            .WithMany(a => a.StatusHistories)
            .HasForeignKey(asc => asc.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(
            appointment => appointment.Period,
            appointmentBuilder =>
            {
                appointmentBuilder.Property(p => p.Start)
                    .HasColumnName("Start");

                appointmentBuilder.Property(p => p.End)
                    .HasColumnName("End");
            });

        builder.Property(asc => asc.CurrentState)
            .HasConversion(
                state => state.Value,
                value => AppointmentCurrentState.From(value).Value!)
            .IsRequired();

        builder.Property(adc => adc.IsCurrentStatus);

        builder.Property(asc => asc.Description)
            .HasMaxLength(200);
    }
}
