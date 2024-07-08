using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Authorization;

public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.ToTable("UserPermissions");

        builder.HasKey(userPermission => new { userPermission.UserId, userPermission.PermissionId });

        // builder.HasOne(userPermission => userPermission.User)
        //     .WithMany(user => user.Permissions)
        //     .HasForeignKey(userPermission => userPermission.UserId)
        //     .OnDelete(DeleteBehavior.Cascade);
        //
        // builder.HasOne(userPermission => userPermission.Permission)
        //     .WithMany(permission => permission.Users)
        //     .HasForeignKey(userPermission => userPermission.PermissionId)
        //     .OnDelete(DeleteBehavior.Cascade);
        builder.Property(userPermission => userPermission.UserId)
            .HasConversion(
                userId => userId.Value,
                value => UserId.From(value))
            .HasColumnName(nameof(UserId))
            .IsRequired();

        builder.Property(userPermission => userPermission.PermissionId)
            .HasConversion(
                permissionId => permissionId.Value,
                value => PermissionId.From(value))
            .HasColumnName(nameof(PermissionId))
            .IsRequired();
    }
}
