using AgendaManager.Domain.Users;
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
            .WithMany(p => p.Roles)
            .UsingEntity(
                typeBuilder =>
                {
                    typeBuilder.ToTable("RolePermissions");
                    typeBuilder.Property<RoleId>("RoleId").HasColumnName("RoleId");
                    typeBuilder.Property<PermissionId>("PermissionId").HasColumnName("PermissionId");
                    typeBuilder.HasKey("RoleId", "PermissionId");

                    // Campos de auditoría.
                    typeBuilder.Property<DateTimeOffset>("CreatedAt");
                    typeBuilder.Property<string>("CreatedBy");
                    typeBuilder.Property<DateTimeOffset>("LastModifiedAt");
                    typeBuilder.Property<string>("LastModifiedBy");
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

        builder.Property(r => r.Editable);
    }
}
