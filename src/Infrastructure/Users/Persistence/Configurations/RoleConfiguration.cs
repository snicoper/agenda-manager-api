using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Users.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Id);

        builder.HasIndex(r => r.Name)
            .IsUnique();

        builder.HasMany(r => r.Permissions)
            .WithMany()
            .UsingEntity(
                typeBuilder =>
                {
                    const string roleIdName = nameof(RoleId);
                    const string permissionIdName = nameof(PermissionId);

                    typeBuilder.ToTable("RolePermissions");
                    typeBuilder.Property<RoleId>(roleIdName).HasColumnName(roleIdName);
                    typeBuilder.Property<PermissionId>(permissionIdName).HasColumnName(permissionIdName);
                    typeBuilder.HasKey(roleIdName, permissionIdName);

                    // Campos de auditoría.
                    typeBuilder.Property<DateTimeOffset>(nameof(AuditableEntity.CreatedAt)).IsRequired();
                    typeBuilder.Property<string>(nameof(AuditableEntity.CreatedBy)).IsRequired();
                    typeBuilder.Property<DateTimeOffset>(nameof(AuditableEntity.LastModifiedAt)).IsRequired();
                    typeBuilder.Property<string>(nameof(AuditableEntity.LastModifiedBy)).IsRequired();
                });

        builder.Property(r => r.Id)
            .HasConversion(
                id => id.Value,
                value => RoleId.From(value))
            .IsRequired();

        builder.Property(r => r.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(r => r.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(r => r.IsEditable);
    }
}
