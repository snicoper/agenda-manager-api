using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Authorization;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(role => role.Id);

        builder.HasIndex(role => role.Name)
            .IsUnique();

        builder.Property(role => role.Id)
            .HasConversion(
                id => id.Value,
                value => RoleId.From(value))
            .IsRequired();

        builder.Property(role => role.Name)
            .HasMaxLength(100)
            .IsRequired();
    }
}
