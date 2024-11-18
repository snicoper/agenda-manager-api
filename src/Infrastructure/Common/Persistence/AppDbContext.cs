﻿using System.Reflection;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.Entities;
using AgendaManager.Domain.AuditRecords;
using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Resources.Entities;
using AgendaManager.Domain.ResourceTypes;
using AgendaManager.Domain.Services;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Common.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Appointment> Appointments => Set<Appointment>();

    public DbSet<AppointmentStatusHistory> AppointmentStatusHistories => Set<AppointmentStatusHistory>();

    public DbSet<Calendar> Calendars => Set<Calendar>();

    public DbSet<CalendarHoliday> CalendarHolidays => Set<CalendarHoliday>();

    public DbSet<CalendarConfiguration> CalendarConfigurations => Set<CalendarConfiguration>();

    public DbSet<AuditRecord> ChangeLogs => Set<AuditRecord>();

    public DbSet<Resource> Resources => Set<Resource>();

    public DbSet<ResourceSchedule> ResourceSchedules => Set<ResourceSchedule>();

    public DbSet<ResourceType> ResourceTypes => Set<ResourceType>();

    public DbSet<Service> Services => Set<Service>();

    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Permission> Permissions => Set<Permission>();

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
