﻿using System.Reflection;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Appointments.Aggregates;
using AgendaManager.Domain.AuditRecords.Aggregates;
using AgendaManager.Domain.Calendars.Aggregates;
using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Resources.Aggregates;
using AgendaManager.Domain.Services.Aggregates;
using AgendaManager.Domain.Users.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Common.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Appointment> Appointments => Set<Appointment>();

    public DbSet<AppointmentStatusChange> AppointmentStatusChanges => Set<AppointmentStatusChange>();

    public DbSet<Calendar> Calendars => Set<Calendar>();

    public DbSet<AuditRecord> ChangeLogs => Set<AuditRecord>();

    public DbSet<CalendarHoliday> CalendarHolidays => Set<CalendarHoliday>();

    public DbSet<Resource> Resources => Set<Resource>();

    public DbSet<ResourceSchedule> ResourceSchedules => Set<ResourceSchedule>();

    public DbSet<ResourceType> ResourceTypes => Set<ResourceType>();

    public DbSet<Permission> Permissions => Set<Permission>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Service> Services => Set<Service>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);

        // Auditable entities.
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (!typeof(IAuditableEntity).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            builder.Entity(entityType.ClrType)
                .Property(nameof(IAuditableEntity.CreatedBy))
                .IsRequired();

            builder.Entity(entityType.ClrType)
                .Property(nameof(IAuditableEntity.CreatedAt))
                .IsRequired();

            builder.Entity(entityType.ClrType)
                .Property(nameof(IAuditableEntity.LastModifiedBy))
                .IsRequired();

            builder.Entity(entityType.ClrType)
                .Property(nameof(IAuditableEntity.LastModifiedAt))
                .IsRequired();

            builder.Entity(entityType.ClrType)
                .Property(nameof(IAuditableEntity.Version))
                .IsRequired();
        }
    }
}
