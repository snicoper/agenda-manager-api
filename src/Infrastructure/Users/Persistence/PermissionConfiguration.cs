using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Users.Persistence;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(p => p.Id);

        builder.HasIndex(p => p.Name)
            .IsUnique();

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PermissionId.From(value))
            .IsRequired();

        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();
    }
}
