using System.Reflection;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Common.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Permission> Permissions => Set<Permission>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<User> Users => Set<User>();

    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    public DbSet<UserRole> UserRoles => Set<UserRole>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
