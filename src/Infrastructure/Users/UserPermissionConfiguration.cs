using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Users;

public class UserPermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("RolePermissions");

        builder.HasKey(up => new { up.RoleId, up.PermissionId });

        builder.HasIndex(up => up.RoleId);
        builder.HasIndex(up => up.PermissionId);

        builder.HasOne(up => up.Role)
            .WithMany("_rolePermissions")
            .HasForeignKey(up => up.RoleId);

        builder.HasOne(up => up.Permission)
            .WithMany("_rolePermissions")
            .HasForeignKey(up => up.PermissionId);

        builder.Property(up => up.RoleId)
            .HasConversion(
                userId => userId.Value,
                value => RoleId.From(value))
            .HasColumnName(nameof(RoleId))
            .IsRequired();

        builder.Property(up => up.PermissionId)
            .HasConversion(
                permissionId => permissionId.Value,
                value => PermissionId.From(value))
            .HasColumnName(nameof(PermissionId))
            .IsRequired();
    }
}
