using System.Reflection;
using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.AuditRecords;
using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.Entities;
using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Common.Messaging;
using AgendaManager.Domain.ResourceManagement.Resources;
using AgendaManager.Domain.ResourceManagement.Resources.Entities;
using AgendaManager.Domain.ResourceManagement.ResourceTypes;
using AgendaManager.Domain.Services;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Common.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IAppDbContext
{
    public DbSet<Appointment> Appointments => Set<Appointment>();

    public DbSet<Calendar> Calendars => Set<Calendar>();

    public DbSet<CalendarHoliday> CalendarHolidays => Set<CalendarHoliday>();

    public DbSet<CalendarSettings> CalendarSettings => Set<CalendarSettings>();

    public DbSet<AuditRecord> ChangeLogs => Set<AuditRecord>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public DbSet<Resource> Resources => Set<Resource>();

    public DbSet<ResourceSchedule> ResourceSchedules => Set<ResourceSchedule>();

    public DbSet<ResourceType> ResourceTypes => Set<ResourceType>();

    public DbSet<Service> Services => Set<Service>();

    public DbSet<User> Users => Set<User>();

    public DbSet<UserRole> UserRoles => Set<UserRole>();

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

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
