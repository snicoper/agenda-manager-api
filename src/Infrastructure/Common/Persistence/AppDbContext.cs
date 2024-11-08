using System.Reflection;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Services;
using AgendaManager.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Common.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Appointment> Appointments => Set<Appointment>();

    public DbSet<AppointmentStatusChange> AppointmentStatusChanges => Set<AppointmentStatusChange>();

    public DbSet<Calendar> Calendars => Set<Calendar>();

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

        // Set row version for auditable entities.
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(IAuditableEntity).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType).Property<uint>(nameof(IAuditableEntity.RowVersion))
                    .IsRowVersion()
                    .HasColumnName("xmin")
                    .HasColumnType("xid");
            }
        }
    }
}
