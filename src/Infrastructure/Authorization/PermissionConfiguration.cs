using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Authorization;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(permission => permission.Id);

        builder.HasIndex(permission => permission.Name)
            .IsUnique();

        builder.Property(permission => permission.Id)
            .HasConversion(
                id => id.Value,
                value => PermissionId.From(value))
            .IsRequired();

        builder.Property(permission => permission.Name)
            .HasMaxLength(100)
            .IsRequired();
    }
}
