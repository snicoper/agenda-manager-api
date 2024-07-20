using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Users;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Id);

        builder.HasIndex(r => r.Name)
            .IsUnique();

        builder
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity(
                j =>
                {
                    j.ToTable("RolePermissions");
                    j.Property<RoleId>("RoleId").HasColumnName("RoleId");
                    j.Property<PermissionId>("PermissionId").HasColumnName("PermissionId");
                    j.HasKey("RoleId", "PermissionId");

                    // Campos de auditoría.
                    j.Property<DateTimeOffset>("CreatedAt");
                    j.Property<string>("CreatedBy");
                    j.Property<DateTimeOffset>("LastModifiedAt");
                    j.Property<string>("LastModifiedBy");
                });

        builder.Property(r => r.Id)
            .HasConversion(
                id => id.Value,
                value => RoleId.From(value))
            .IsRequired();

        builder.Property(r => r.Name)
            .HasMaxLength(100)
            .IsRequired();
    }
}
