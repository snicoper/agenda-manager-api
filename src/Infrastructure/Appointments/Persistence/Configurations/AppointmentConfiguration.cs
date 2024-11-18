using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Resources.ValueObjects;
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
            .Property(a => a.UserId)
            .HasConversion(
                userId => userId.Value,
                value => UserId.From(value))
            .IsRequired();

        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.StatusHistories)
            .WithOne(sc => sc.Appointment)
            .HasForeignKey(sc => sc.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(
            a => a.Period,
            appointmentBuilder =>
            {
                appointmentBuilder.Property(p => p.Start)
                    .HasColumnName("Start");

                appointmentBuilder.Property(p => p.End)
                    .HasColumnName("End");
            });

        builder.Property(a => a.CurrentState)
            .HasConversion(
                state => state.Value,
                value => AppointmentCurrentState.From(value).Value!)
            .IsRequired();

        builder.HasMany(a => a.StatusHistories)
            .WithOne(asc => asc.Appointment)
            .HasForeignKey(asc => asc.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Resources)
            .WithMany()
            .UsingEntity(
                typeBuilder =>
                {
                    const string appointmentIdName = nameof(AppointmentId);
                    const string resourceIdName = nameof(ResourceId);

                    typeBuilder.ToTable("AppointmentResources");
                    typeBuilder.Property<AppointmentId>(appointmentIdName).HasColumnName(appointmentIdName);
                    typeBuilder.Property<ResourceId>(resourceIdName).HasColumnName(resourceIdName);
                    typeBuilder.HasKey(appointmentIdName, resourceIdName);

                    // Campos de auditoría.
                    typeBuilder.Property<DateTimeOffset>(nameof(AuditableEntity.CreatedAt)).IsRequired();
                    typeBuilder.Property<string>(nameof(AuditableEntity.CreatedBy)).IsRequired();
                    typeBuilder.Property<DateTimeOffset>(nameof(AuditableEntity.LastModifiedAt)).IsRequired();
                    typeBuilder.Property<string>(nameof(AuditableEntity.LastModifiedBy)).IsRequired();
                });
    }
}
