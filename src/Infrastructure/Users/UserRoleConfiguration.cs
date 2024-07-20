using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Users;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(ur => new { ur.UserId, ur.RoleId });

        builder.HasIndex(ur => ur.UserId);
        builder.HasIndex(ur => ur.RoleId);

        builder.HasOne(ur => ur.User)
            .WithMany("_userRoles")
            .HasForeignKey(ur => ur.UserId);

        builder.HasOne(ur => ur.Role)
            .WithMany("_userRoles")
            .HasForeignKey(ur => ur.RoleId);

        builder.Property(ur => ur.UserId)
            .HasConversion(
                userId => userId.Value,
                value => UserId.From(value))
            .HasColumnName(nameof(UserId))
            .IsRequired();

        builder.Property(ur => ur.RoleId)
            .HasConversion(
                roleId => roleId.Value,
                value => RoleId.From(value))
            .HasColumnName(nameof(RoleId))
            .IsRequired();
    }
}
