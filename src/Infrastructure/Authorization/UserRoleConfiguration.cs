using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Authorization;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(userRole => new { userRole.UserId, userRole.RoleId });

        builder.Property(userRole => userRole.UserId)
            .HasConversion(
                userId => userId.Value,
                value => UserId.From(value))
            .HasColumnName(nameof(UserId))
            .IsRequired();

        builder.Property(userRole => userRole.RoleId)
            .HasConversion(
                roleId => roleId.Value,
                value => RoleId.From(value))
            .HasColumnName(nameof(RoleId))
            .IsRequired();
    }
}
